using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizzApp.Data;
using QuizzApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace QuizzApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Questions
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Question.Include(q => q.Quiz);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Questions/ListForQuiz/5
        [Authorize]
        public async Task<IActionResult> ListForQuiz(int quizId)
        {
            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (quiz.UserId != userId)
                return Forbid();

            return View(quiz);
        }


        // GET: Questions/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        [Authorize]
        public IActionResult Create(int quizId)
        {
            // Check if the quiz exists
            var quiz = _context.Quiz.Find(quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            var question = new Question
            {
                QuizId = quizId,
                Quiz = quiz // Set the Quiz navigation property
            };
            return View(question);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Text,Score,QuizId")] Question question)
        {
            var quiz = await _context.Quiz.FindAsync(question.QuizId);
            if (quiz == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (quiz.UserId != userId)
            {
                return Forbid(); // User is not authorized to create questions for this quiz
            }
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("ListForQuiz", new { quizId = question.QuizId });
            }
            return View(question);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.Quiz == null || question.Quiz.UserId != userId)
            {
                return Forbid();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,QuizId,Score")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }
            var existingQuestion = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (existingQuestion == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existingQuestion.Quiz == null || existingQuestion.Quiz.UserId != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                existingQuestion.Text = question.Text;
                existingQuestion.Score = question.Score;
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ListForQuiz", new { quizId = existingQuestion.QuizId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.Quiz == null || question.Quiz.UserId != userId)
            {
                return Forbid(); 
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == id);
            if (question==null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.Quiz == null || question.Quiz.UserId != userId)
            {
                return Forbid();
            }
            var quizId = question.QuizId;
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListForQuiz", new { quizId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
