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

namespace LKSApi.Controllers
{
    public class tbldetailsController : ApiController
    {
        private lksEntities db = new lksEntities();

        // GET: api/tbldetails
        public IQueryable<tbldetail> Gettbldetails()
        {
            return db.tbldetails;
        }

        // GET: api/tbldetails/5
        [ResponseType(typeof(tbldetail))]
        public IHttpActionResult Gettbldetail(int id)
        {
            tbldetail tbldetail = db.tbldetails.Find(id);
            if (tbldetail == null)
            {
                return NotFound();
            }

            return Ok(tbldetail);
        }

        // PUT: api/tbldetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbldetail(int id, tbldetail tbldetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbldetail.iddetail)
            {
                return BadRequest();
            }

            db.Entry(tbldetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbldetailExists(id))
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

        // POST: api/tbldetails
        [ResponseType(typeof(tbldetail))]
        public IHttpActionResult Posttbldetail(tbldetail tbldetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbldetails.Add(tbldetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbldetail.iddetail }, tbldetail);
        }

        // DELETE: api/tbldetails/5
        [ResponseType(typeof(tbldetail))]
        public IHttpActionResult Deletetbldetail(int id)
        {
            tbldetail tbldetail = db.tbldetails.Find(id);
            if (tbldetail == null)
            {
                return NotFound();
            }

            db.tbldetails.Remove(tbldetail);
            db.SaveChanges();

            return Ok(tbldetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbldetailExists(int id)
        {
            return db.tbldetails.Count(e => e.iddetail == id) > 0;
        }
    }
}