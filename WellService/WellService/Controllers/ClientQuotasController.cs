using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WellService.Helpers;
using WellService.Models;

namespace WellService.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ClientQuotasController : Controller
    {
        private readonly WellDbContext _context;

        public ClientQuotasController(WellDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Get Quota by Client Name
        /// </summary>
        /// <param name="ClientName"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetQuotaByClientName(string ClientName)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.ClientQuotas
                            where x.ClientName.ToLower().Contains(ClientName.ToLower())
                            orderby x.ClientID ascending
                            select x;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        /// <summary>
        /// Get All Client Data
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ActionResult GetAllClients()
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                var datas = from x in _context.ClientQuotas
                            orderby x.ClientID ascending
                            select x;
                hasil.Data = datas.ToList();
            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        /// <summary>
        /// Update quota value
        /// </summary>
        /// <param name="ClientData"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> UpdateClientQuota([FromBody] ClientQuota ClientData)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (ClientData != null)
                {

                    var datas = from x in _context.ClientQuotas
                                where x.ClientID == ClientData.ClientID
                                select x;

                    if (datas != null)
                    {
                        foreach (var item in datas)
                        {
                            item.Quota = ClientData.Quota;
                        }
                        var res = await _context.SaveChangesAsync();
                        hasil.Data = true;
                        hasil.IsSucceed = res > 0 ? true : false;
                    }
                    else
                    {
                        hasil.IsSucceed = false;
                    }
                }
                else
                {
                    hasil.IsSucceed = false;
                }

            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }

        /// <summary>
        /// Add or Update Client (provide client id if you want to update, otherwise it will add a new record
        /// </summary>
        /// <param name="NewClient"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult> AddOrUpdateClient([FromBody] ClientQuota NewClient)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                if (NewClient != null)
                {

                    var exists = _context.ClientQuotas.Any(a => a.ClientID == NewClient.ClientID);
                    if (exists)
                    {
                        var itemSel = _context.ClientQuotas.SingleOrDefault(x => x.ClientID == NewClient.ClientID);
                        itemSel.ClientName = NewClient.ClientName;
                        itemSel.Email = NewClient.Email;
                        itemSel.Quota = NewClient.Quota;
                    }
                    else
                    {
                        _context.ClientQuotas.Add(NewClient);
                    }

                    var res = await _context.SaveChangesAsync();
                    hasil.Data = true;
                    hasil.IsSucceed = res > 0 ? true : false;

                }
                else
                {
                    hasil.IsSucceed = false;
                }

            }
            catch (Exception ex)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = ex.Message;
            }
            return Ok(hasil);
        }
    }
}
