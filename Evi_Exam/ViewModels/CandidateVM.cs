using Evi_Exam.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Evi_Exam.ViewModels
{
    public class CandidateVM
    {
        public CandidateVM()
        {
            this.SkillList = new List<int>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public int Phone { get; set; }
        [Display(Name ="Image")]
        public IFormFile ImagePath { get; set; }
        public string Image { get; set; }
        public bool Fresher { get; set; }

        public List<int> SkillList { get; set; }
    }
}
