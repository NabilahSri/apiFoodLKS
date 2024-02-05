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
    public class tbltransController : ApiController
    {
        private lksEntities db = new lksEntities();

        // GET: api/tbltrans
        public IQueryable<tbltran> Gettbltrans()
        {
            return db.tbltrans;
        }

        // GET: api/tbltrans
        [ActionName("idtrans")]
        [Route("idtrans")]
        [HttpGet]
        public IQueryable<tbltran> idtrans()
        {
            IQueryable<tbltran> query = db.tbltrans.OrderByDescending(e => e.idtrans).Take(1);
            return query;
        }

        // GET: api/tbltrans/5
        [ResponseType(typeof(tbltran))]
        public IHttpActionResult Gettbltran(string id)
        {
            tbltran tbltran = db.tbltrans.Find(id);
            if (tbltran == null)
            {
                return NotFound();
            }

            return Ok(tbltran);
        }

        // PUT: api/tbltrans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbltran(string id, tbltran tbltran)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbltran.idtrans)
            {
                return BadRequest();
            }

            db.Entry(tbltran).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbltranExists(id))
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

        // POST: api/tbltrans
        [ResponseType(typeof(tbltran))]
        public IHttpActionResult Posttbltran(tbltran tbltran)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbltrans.Add(tbltran);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbltranExists(tbltran.idtrans))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbltran.idtrans }, tbltran);
        }

        // DELETE: api/tbltrans/5
        [ResponseType(typeof(tbltran))]
        public IHttpActionResult Deletetbltran(string id)
        {
            tbltran tbltran = db.tbltrans.Find(id);
            if (tbltran == null)
            {
                return NotFound();
            }

            db.tbltrans.Remove(tbltran);
            db.SaveChanges();

            return Ok(tbltran);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbltranExists(string id)
        {
            return db.tbltrans.Count(e => e.idtrans == id) > 0;
        }
    }
}