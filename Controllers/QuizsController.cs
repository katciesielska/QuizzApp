using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizzApp.Data;
using QuizzApp.Models;

namespace QuizzApp.Controllers
{
    public class QuizsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quizs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Quiz.Include(q => q.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Quizs/MyQuizzes
        [Authorize]
        public async Task<IActionResult> MyQuizzes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myQuizzes = _context.Quiz
            .Include(q => q.User)
            .Where(q => q.UserId == userId);
            return View(await myQuizzes.ToListAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
                return NotFound();

            return View(quiz);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitQuiz(int QuizId, Dictionary<int, int> SelectedAnswers)
        {
            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == QuizId);

            if (quiz == null)
                return NotFound();

            int score = 0;
            foreach (var question in quiz.Questions)
            {
                if (SelectedAnswers.TryGetValue(question.Id, out int answerId))
                {
                    var answer = question.Answers.FirstOrDefault(a => a.Id == answerId);
                    if (answer != null && answer.IsCorrect)
                    {
                        score += question.Score;
                    }
                }
            }

            ViewData["Score"] = score;
            ViewData["MaxScore"] = quiz.Questions.Sum(q => q.Score);
            return View("QuizResult", quiz);
        }


        // GET: Quizs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quizs/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Quizs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                quiz.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                // After saving the quiz
                return RedirectToAction("Create", "Questions", new { quizId = quiz.Id });
            }
            return View(quiz);

        }

        // GET: Quizs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (quiz.UserId != userId)
            {
                return Forbid(); // User is not authorized to edit this quiz
            }
            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return View(quiz);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var quizToUpdate = await _context.Quiz.FindAsync(id);

            if (quizToUpdate == null)
            {
                return NotFound();
            }

            // Only owner can edit
            if (quizToUpdate.UserId != userId)
            {
                return Forbid();
            }

            // Update only allowed fields
            quizToUpdate.Title = quiz.Title;
            quizToUpdate.Description = quiz.Description;

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(quiz.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // GET: Quizs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
            return _context.Quiz.Any(e => e.Id == id);
        }
    }
}
