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

namespace LKSApi.Controllers
{
    public class tblbarangsController : ApiController
    {
        private lksEntities db = new lksEntities();

        // GET: api/tblbarangs
        public IQueryable<tblbarang> Gettblbarangs()
        {
            return db.tblbarangs;
        }

        // GET: api/tblbarangs/5
        [ResponseType(typeof(tblbarang))]
        public IHttpActionResult Gettblbarang(string id)
        {
            tblbarang tblbarang = db.tblbarangs.Find(id);
            if (tblbarang == null)
            {
                return NotFound();
            }

            return Ok(tblbarang);
        }

        // PUT: api/tblbarangs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttblbarang(string id, tblbarang tblbarang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tblbarang.idbarang)
            {
                return BadRequest();
            }

            db.Entry(tblbarang).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblbarangExists(id))
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

        // POST: api/tblbarangs
        [ResponseType(typeof(tblbarang))]
        public IHttpActionResult Posttblbarang(tblbarang tblbarang)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tblbarangs.Add(tblbarang);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tblbarangExists(tblbarang.idbarang))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tblbarang.idbarang }, tblbarang);
        }

        // DELETE: api/tblbarangs/5
        [ResponseType(typeof(tblbarang))]
        public IHttpActionResult Deletetblbarang(string id)
        {
            tblbarang tblbarang = db.tblbarangs.Find(id);
            if (tblbarang == null)
            {
                return NotFound();
            }

            db.tblbarangs.Remove(tblbarang);
            db.SaveChanges();

            return Ok(tblbarang);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tblbarangExists(string id)
        {
            return db.tblbarangs.Count(e => e.idbarang == id) > 0;
        }

        [Route("barang/image/{name}")]
        [HttpGet]
        public IHttpActionResult GetImage(string name)
        {
            string filename = @"C:\Users\RPL\source\repos\LKS\LKS\barang\" + name + ".jpg";
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
    }
}