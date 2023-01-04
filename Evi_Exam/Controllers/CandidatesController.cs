using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Evi_Exam.Models;
using Microsoft.AspNetCore.Hosting;
using Evi_Exam.ViewModels;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis;

namespace Evi_Exam.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly CandidateDbContext _context;
        private readonly IWebHostEnvironment _he;

        public CandidatesController(CandidateDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidates.Include(x => x.CandidateSkills).ThenInclude(y => y.Skills).ToListAsync());
        }


        public IActionResult AddNewSkills(int? id)
        {
            ViewBag.skill = new SelectList(_context.Skills, "SkillsId", "SkillsName", id.ToString() ?? "");

            return PartialView("_AddNewSkills");
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateVM candidateVM, int[] SkillsId)
        {
            if (ModelState.IsValid)
            {

                Candidate candidate = new Candidate()
                {

                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Fresher = candidateVM.Fresher,


                };
                var file = candidateVM.ImagePath;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);
                if (file!=null)
                {
                    using (var strem = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImagePath.CopyTo(strem);
                        candidate.Image = "/" + folder + "/" + imgFileName;
                    }

                }
    

                foreach (var item in SkillsId)
                {
                    CandidateSkills candidateSkills = new CandidateSkills()
                    {
                        Candidate = candidate,
                        CandidateId = candidate.CandidateId,
                        SkillsId = item
                    };
                    _context.CandidateSkills.Add(candidateSkills);

                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public async Task<IActionResult> Edit(int? id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x=>x.CandidateId==id);
       
            CandidateVM candidateVM =new CandidateVM() { 
                CandidateId= candidate.CandidateId,
                CandidateName= candidate.CandidateName,
                DateOfBirth= candidate.DateOfBirth,
                Phone= candidate.Phone,
                Image= candidate.Image,
                Fresher= candidate.Fresher,
             
           };

            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
            foreach (var item in existSkill)
            {
                candidateVM.SkillList.Add(item.SkillsId);
            }

            return View(candidateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CandidateVM candidateVM, int[] SkillsId)
        {
            

            if (ModelState.IsValid)
            {
                Candidate candidate = new Candidate()
                {
                    CandidateId= candidateVM.CandidateId,
                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Fresher = candidateVM.Fresher,
                    Image= candidateVM.Image
                };
                var file = candidateVM.ImagePath;
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(candidateVM.ImagePath.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var strem = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImagePath.CopyTo(strem);
                        candidate.Image = "/" + folder + "/" + imgFileName;
                    }

                }
              
                var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
                foreach (var item in existSkill)
                {
                    _context.CandidateSkills.Remove(item);
                }

                foreach (var item in SkillsId)
                {
                    CandidateSkills candidateSkills = new CandidateSkills()
                    {
                       
                        CandidateId = candidate.CandidateId,
                        SkillsId = item
                    };
                    _context.CandidateSkills.Add(candidateSkills);

                }

                _context.Update(candidate);
                await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var candidate = await _context.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
            var existSkill = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();
            foreach (var item in existSkill)
            {
                _context.CandidateSkills.Remove(item);
            }

            _context.Remove(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }

        //    <script>
        //    function LoadData(SkillsId = null)
        //    {
        //        var SkillContainer = $("#SkillContainer")
        //        $.ajax({
        //        url: "/Candidates/AddNewSkills/" + SkillsId ?? "",
        //            type: "GET",
        //            success: function(data) {
        //                SkillContainer.append(data);
        //            }
        //        });
        //    }

        //    $(document).on("click", "#BtnPlus", function (e) {
        //        e.preventDefault();
        //        LoadData();
        //    });
        //    $(document).on("click", "#DeleteSkills", function (e) {
        //        e.preventDefault();
        //        $(this).parent().parent().remove();
        //    });
        //</script>

        //@foreach(var item in Model.SkillList)
        //{
        //            < script >
        //                LoadData(@item);
        //            </ script >
        //        }


        //   <Project Sdk = "Microsoft.NET.Sdk.Web">
        //  <PropertyGroup>
        //    <TargetFramework> netcoreapp3.1</TargetFramework>
        //  </PropertyGroup>
        //  <ItemGroup>
        //    <PackageReference Include = "Microsoft.AspNetCore.Mvc.TagHelpers" Version="2.2.0" />
        //    <PackageReference Include = "Microsoft.EntityFrameworkCore" Version="3.1.32" />
        //    <PackageReference Include = "Microsoft.EntityFrameworkCore.Relational" Version="3.1.32" />
        //    <PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.32" />
        //    <PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version="3.1.32">
        //      <PrivateAssets>all</PrivateAssets>
        //      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
        //    </PackageReference>
        //    <PackageReference Include = "Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="3.1.5" />
        //  </ItemGroup>
        //</Project>



        //endpoints.MapControllerRoute(
        //        name: "Ami",
        //        pattern: "Ami",
        //        defaults: new { controller = "Candidates", action = "Index" });


    }
}
