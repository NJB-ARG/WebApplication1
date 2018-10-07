using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models.ViewModels
{
    public class Message
    {
        //public Message()
        //{
        //    Message_Tarea = new Tarea();
        //}

        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string MessageToPost { get; set; }
        public string From { get; set; }
        public DateTime DatePosted { get; set; }

        [Display(Name = "Tarea ID")]
        public int? TareaID { get; set; }
        //Navigation Properties - un solo objeto
        [Display(Name = "Mensaje Tarea")]
        public virtual Tarea Message_Tarea { get; set; }

    }

    public class Reply
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string ReplyFrom { get; set; }
        [Required]
        public string ReplyMessage { get; set; }
        public DateTime ReplyDateTime { get; set; }
    }
}