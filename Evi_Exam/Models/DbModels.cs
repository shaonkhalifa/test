using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evi_Exam.Models
{
    public class Skills
    {
        public Skills()
        {
            this.CandidateSkills=new List<CandidateSkills>();
        }
        public int SkillsId { get; set; }
        public string SkillsName { get; set;}
        public virtual ICollection<CandidateSkills> CandidateSkills { get; set; }
    }
    public class Candidate
    {
        public Candidate()
        {
            this.CandidateSkills = new List<CandidateSkills>();
        }
        public int CandidateId { get; set; }
        public string CandidateName { get; set;}
        [DataType(DataType.Date),DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public int Phone { get; set; }
        public string Image { get; set; }
        public bool Fresher { get; set; }

        public virtual ICollection<CandidateSkills> CandidateSkills { get; set; }
    }

    public class CandidateSkills
    {
        public int CandidateSkillsId { get; set; }
        [ForeignKey("Skills")]
        public int SkillsId { get; set; }
        [ForeignKey("Candidate")]
        public int CandidateId { get; set; }
        public virtual Skills Skills  { get; set; }

        public virtual Candidate Candidate  { get; set; }
    }
    public class CandidateDbContext : DbContext
    {
        public CandidateDbContext(DbContextOptions<CandidateDbContext> options):base(options) { }
       
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkills> CandidateSkills { get; set; }

    }
}
