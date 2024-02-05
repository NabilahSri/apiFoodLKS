using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LKSApi.Models;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace LKSApi.Controllers
{
    public class tblusersController : ApiController
    {
        private lksEntities db = new lksEntities();

        // GET: api/tblusers
        [Authorize]
        public IQueryable<tbluser> Gettblusers()
        {
            return db.tblusers;
        }

        // GET: api/tblusers/5
        [ResponseType(typeof(tbluser))]
        public IHttpActionResult Gettbluser(int id)
        {
            tbluser tbluser = db.tblusers.Find(id);
            if (tbluser == null)
            {
                return NotFound();
            }

            return Ok(tbluser);
        }

        // PUT: api/tblusers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbluser(int id, tbluser tbluser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbluser.iduser)
            {
                return BadRequest();
            }

            db.Entry(tbluser).State = EntityState.Modified;

            try
            {

                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbluserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tblusers
        [ResponseType(typeof(tbluser))]
        public IHttpActionResult Posttbluser(tbluser tbluser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (registerExists(tbluser.username))
            {
                var data = new Dictionary<String, String>();
                data.Add("status", "failed");
                return Ok(data);
            }
            else
            {
                db.tblusers.Add(tbluser);
                db.SaveChanges();
                var data = new Dictionary<String, String>();
                data.Add("status", "success");
                return Ok(data);
            }
        }

        // POST: login
        [ActionName("login")]
        [Route("login")]
        [HttpPost]
        [ResponseType(typeof(tbluser))]
        public IHttpActionResult login(tbluser tbluser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (loginExists(tbluser.username, tbluser.pass))
            {
                tbluser users = db.tblusers.Where(e => e.username == tbluser.username && e.pass == tbluser.pass).FirstOrDefault();
                var data = new Dictionary<String, Object>();
                data.Add("status", "success");
                data.Add("users", users);
                return Ok(data);
            }
            else
            {
                var data = new Dictionary<String, String>();
                data.Add("status", "failed");
                return Ok(data);
            }
        }

        // DELETE: api/tblusers/5
        [ResponseType(typeof(tbluser))]
        public IHttpActionResult Deletetbluser(int id)
        {
            tbluser tbluser = db.tblusers.Find(id);
            if (tbluser == null)
            {
                return NotFound();
            }

            db.tblusers.Remove(tbluser);
            db.SaveChanges();

            return Ok(tbluser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbluserExists(int id)
        {
            return db.tblusers.Count(e => e.iduser == id) > 0;
        }

        private bool loginExists(string user,string pass)
        {
            return db.tblusers.Count(e => e.username == user && e.pass==pass) > 0;
        }

        private bool registerExists(string user)
        {
            return db.tblusers.Count(e => e.username == user) > 0;
        }

        [Route("users/image/{name}")]
        [HttpGet]
        public IHttpActionResult GetImage(string name)
        {
            string filename = @"C:\Users\RPL\source\repos\LKS\LKS\img\" + name + ".jpg";
            HttpResponseMessage response;
            if (File.Exists(filename))
            {
                var stream = File.OpenRead(filename);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                response.Content.Headers.ContentLength = stream.Length;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return ResponseMessage(response);
        }

        [Route("users/image")]
        [HttpPost]
        [ResponseType(typeof(tbluser))]
        public IHttpActionResult SetImage([FromBody] JObject requestBody)
        {
            try
            {
                string encodeImage = (string) requestBody["image"];
                string nama = (string) requestBody["iduser"];

                byte[] imageByte = Convert.FromBase64String(encodeImage);

                using(MemoryStream ms = new MemoryStream(imageByte))
                {
                    using (Image image = Image.FromStream(ms))
                    {
                        string imageName = nama.ToString()+".jpg";
                        string imagePath = @"C:\Users\RPL\source\repos\LKS\LKS\img\" + imageName;
                        image.Save(imagePath, ImageFormat.Jpeg);

                        var data = new Dictionary<String, Object>();
                        data.Add("status", "success");
                        return Ok(data);
                    }
                }
            }catch
            {
                var data = new Dictionary<String, Object>();
                data.Add("status", "failed");
                return Ok(data);
            }
        }
    }
}