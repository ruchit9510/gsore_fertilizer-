﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Vendor.Models
{
    public class BlogModel
    {
        [Key]
        public int BlogId { get; set; }
        [Required(ErrorMessage = "Blog Title Required")]
        public string BlogTitle { get; set; }
        [Required(ErrorMessage = "Blog Date Required")]
        public DateTime? BlogDate { get; set; }
        [Required(ErrorMessage = "Auther Name Required")]
        public string BlogAutherName { get; set; }
        [Required(ErrorMessage = "Blog Description Required")]

        [AllowHtml] // ✅ This allows HTML input
        public string BlogDescription { get; set; }
    }
}