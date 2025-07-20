using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizzApp.Data;
using QuizzApp.Models;

namespace QuizzApp.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AnswersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Answers
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int questionId)
        {
            var question = await _context.Question.Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // GET: Answers/Details/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .ThenInclude(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            if (!await IsOwnerAsync(answer.Question))
            {
                return Forbid();
            }

            return View(answer);
        }

        // GET: Answers/Create
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int questionId)
        {
            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(question))
            {
                return Forbid();
            }
            var answer = new Answer
            {
                QuestionId = questionId
            };
            return View(answer);
        }

        // POST: Answers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,QuestionId")] Answer answer)
        {
            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(question))
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { questionId = answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answers/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .ThenInclude(q => q.Quiz)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(answer.Question))
            {
                return Forbid();
            }
            return View(answer);
        }

        // POST: Answers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,QuestionId")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == answer.QuestionId);
            if (question == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(question))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { questionId = answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answers/Delete/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Question)
                .ThenInclude(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(answer.Question))
            {
                return Forbid();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answer
                .Include(a => a.Question)
                .ThenInclude(q => q.Quiz)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (answer == null)
            {
                return NotFound();
            }
            if (!await IsOwnerAsync(answer.Question))
            {
                return Forbid();
            }

            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { questionId = answer.QuestionId });
        }

        private bool AnswerExists(int id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }

        private async Task<bool> IsOwnerAsync(Question? question)
        {
            if (question == null)
                return false;

            var quiz = question.Quiz ?? await _context.Quiz.FirstOrDefaultAsync(q => q.Id == question.QuizId);
            if (quiz == null)
                return false;

            var userId = _userManager.GetUserId(User);
            return quiz.UserId == userId;
        }
    }
}
