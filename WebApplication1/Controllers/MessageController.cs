using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models;
using SendGrid;
using System.Net;
using System.Net.Mail;
using WebApplication1.Controllers;
using SendGrid.SmtpApi;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Data.Entity;

namespace WebApplication1.Controllers
{
    public class MessageController : ApplicationBaseController
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        [HttpPost]
        [Authorize]
        public ActionResult PostMessage(MessageReplyViewModel vm, int? idTarea)
        {
            var username = User.Identity.Name;
            string fullName = "";
            int msgid = 0;
            if (!string.IsNullOrEmpty(username))
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserName == username);
                fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
            }
            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = fullName;
                messagetoPost.TareaID = idTarea;                

                dbContext.Messages.Add(messagetoPost);
                //dbContext.Entry(messagetoPost.Message_Tarea).State = EntityState.Unchanged;
                dbContext.SaveChanges();
                msgid = messagetoPost.Id;
            }

            return RedirectToAction("Index", "Mensajes", new { Id = msgid, idTarea = idTarea });
        }

        public ActionResult Create(int? idTarea)
        {
            MessageReplyViewModel vm = new MessageReplyViewModel();
            //NJB-ID de la tarea            
            ViewBag.Desc1 = dbContext.Tareas.Where(x => x.TareaID == idTarea).Select(s => s.TareaDescripcion).FirstOrDefault();
            ViewBag.Desc2 = dbContext.Tareas.Where(x => x.TareaID == idTarea).Select(s => s.TareaDescripcion2).FirstOrDefault();
            ViewBag.idTarea = idTarea;
            //
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ReplyMessage(MessageReplyViewModel vm, int messageId)
        {
            var username = User.Identity.Name;
            string fullName = "";
            if (!string.IsNullOrEmpty(username))
            {
                var user = dbContext.Users.SingleOrDefault(u => u.UserName == username);
                fullName = string.Concat(new string[] { user.FirstName, " ", user.LastName });
            }
            if (vm.Reply.ReplyMessage != null)
            {
                Reply _reply = new Reply();
                _reply.ReplyDateTime = DateTime.Now;
                _reply.MessageId = messageId;
                _reply.ReplyFrom = fullName;
                _reply.ReplyMessage = vm.Reply.ReplyMessage;
                dbContext.Replies.Add(_reply);
                dbContext.SaveChanges();
            }
            //reply to the message owner          - using email template

            var messageOwner = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            var users = from user in dbContext.Users
                        orderby user.FirstName
                        select new
                        {
                            FullName = user.FirstName + " " + user.LastName,
                            UserEmail = user.Email
                        };

            var uemail = users.Where(x => x.FullName == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
            //NJB-Comentado INI
            //SendGridMessage replyMessage = new SendGridMessage();
            //replyMessage.From = new EmailAddress(username);
            //replyMessage.Subject = "Reply for your message :" + dbContext.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault();
            //replyMessage.PlainTextContent = vm.Reply.ReplyMessage;

            //replyMessage.AddTo(uemail);

            //var credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"],
            //                                        ConfigurationManager.AppSettings["mailPassword"]);
            //var transportweb = new Web(credentials);
            //transportweb.DeliverAsync(replyMessage);
            //NJB-Comentado FIN

            var idTarea = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.TareaID).FirstOrDefault();

            //
            // Retrieve the API key from the environment variables. See the project README for more info about setting this up.
            var apiKey = Environment.GetEnvironmentVariable("api_key_name_NJB1");

            var client = new SendGridClient(apiKey);

            // Send a Single Email using the Mail Helper
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(username),
                Subject = "Reply for your message :" + dbContext.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault(),
                PlainTextContent = vm.Reply.ReplyMessage,
                HtmlContent = vm.Reply.ReplyMessage,
        };
            msg.AddTo(uemail);

            var response = client.SendEmailAsync(msg);
            //

            return RedirectToAction("Index", "Mensajes", new { Id = messageId, idTarea = idTarea });

        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteMessage(int messageId, int? idTarea)
        {
            Message _messageToDelete = dbContext.Messages.Find(messageId);
            dbContext.Messages.Remove(_messageToDelete);
            dbContext.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = dbContext.Replies.Where(i => i.MessageId == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    dbContext.Replies.Remove(rep);
                    dbContext.SaveChanges();
                }
            }

            //var idTarea = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.TareaID).FirstOrDefault();


            return RedirectToAction("Index", "Mensajes", new { idTarea = idTarea });
        }
    }
}