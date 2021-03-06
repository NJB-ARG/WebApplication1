﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace WebApplication1.Controllers
{
    [RequireHttps]
    public class MensajesController : ApplicationBaseController
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index(int? Id, int? page, int? idTarea)
        {            
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();            
            var count = dbContext.Messages.Count();
            //NJB
            if (idTarea != null)
            {
                count = dbContext.Messages.Where(x=> x.TareaID == idTarea.Value).Count();
            }
            //NJB            

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = dbContext.Messages.Where(x => x.TareaID == idTarea.Value)
                                       .OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = dbContext.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = dbContext.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }
            //NJB-ID de la tarea
            ViewBag.idTarea = idTarea;
            ViewBag.Desc1 = dbContext.Tareas.Where(x => x.TareaID == idTarea).Select(s=> s.TareaDescripcion).FirstOrDefault();
            ViewBag.Desc2 = dbContext.Tareas.Where(x => x.TareaID == idTarea).Select(s => s.TareaDescripcion2).FirstOrDefault();
            //
            return View(vm);
        }
    }
}