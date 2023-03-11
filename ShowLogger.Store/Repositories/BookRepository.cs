using ShowLogger.Data.Context;
using ShowLogger.Data.Entities;
using ShowLogger.Models;
using ShowLogger.Store.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories;
public class BookRepository : IBookRepository
{
    private readonly ShowLoggerDbContext _context;

    public BookRepository(ShowLoggerDbContext context)
    {
        _context = context;
    }

    public IEnumerable<BookModel> GetBooks(Expression<Func<BookModel, bool>>? predicate)
    {
        IEnumerable<BookModel> query = _context.SL_BOOK.Select(m => new BookModel
        {
            BookId = m.BOOK_ID,
            UserId = m.USER_ID,
            BookName = m.BOOK_NAME,
            StartDate = m.START_DATE,
            EndDate = m.END_DATE,
            Chapters = m.CHAPTERS,
            Pages = m.PAGES,
            BookNotes = m.BOOK_NOTES,
        });

        if (predicate != null)
        {
            query = query.AsQueryable().Where(predicate);
        }

        return query;
    }

    public long CreateBook(int userId, BookModel model)
    {
        long id = 0;

        SL_BOOK entity = new SL_BOOK
        {
            BOOK_NAME = model.BookName,
            START_DATE = model.StartDate,
            END_DATE = model.EndDate,
            CHAPTERS = model.Chapters,
            PAGES = model.Pages,
            BOOK_NOTES = model.BookNotes,
            USER_ID = userId
        };

        _context.SL_BOOK.Add(entity);
        _context.SaveChanges();
        id = entity.BOOK_ID;

        return id;
    }

    public long UpdateBook(int userId, BookModel model)
    {
        SL_BOOK? entity = _context.SL_BOOK.FirstOrDefault(m => m.BOOK_ID == model.BookId && m.USER_ID == userId);

        if (entity != null)
        {
            entity.BOOK_NAME = model.BookName;
            entity.START_DATE = model.StartDate;
            entity.END_DATE = model.EndDate;
            entity.CHAPTERS = model.Chapters;
            entity.PAGES = model.Pages;
            entity.BOOK_NOTES = model.BookNotes;

            return _context.SaveChanges();
        }
        else
            return 0;
    }

    public bool DeleteBook(int userId, int showId)
    {
        SL_BOOK? entity = _context.SL_BOOK.FirstOrDefault(m => m.BOOK_ID == showId && m.USER_ID == userId);

        if (entity != null)
        {
            _context.SL_BOOK.Remove(entity);

            _context.SaveChanges();

            return true;
        }
        else
            return false;
    }

    public IEnumerable<YearStatsBookModel> GetYearStats(int userId)
    {
        int[] friends = _context.SL_FRIEND.Where(m => m.USER_ID == userId).Select(m => m.FRIEND_USER_ID)
            .Union(_context.SL_FRIEND.Where(m => m.FRIEND_USER_ID == userId).Select(m => m.USER_ID)).ToArray();

        SL_BOOK[] books = _context.SL_BOOK.ToArray();

        IEnumerable<YearStatsBookModel> model = from u in _context.OA_USERS.AsEnumerable()
                                                join x in books on u.USER_ID equals x.USER_ID
                                                where x.END_DATE != null && x.START_DATE != null
                                                group new { x, u } by new { x.USER_ID, x.END_DATE.Value.Year, u.FIRST_NAME, u.LAST_NAME, u.USER_NAME } into g
                                                select new YearStatsBookModel
                                                {
                                                    UserId = g.Key.USER_ID,
                                                    Name = $"{g.Key.LAST_NAME}, {g.Key.FIRST_NAME}",
                                                    Year = g.Key.Year,
                                                    BookCnt = g.Count(),
                                                    ChapterCnt = g.Sum(m => m.x.CHAPTERS) ?? 0,
                                                    PageCnt = g.Sum(m => m.x.PAGES) ?? 0,
                                                    TotalDays = (decimal)g.Sum(m => (m.x.END_DATE.Value - m.x.START_DATE.Value).TotalDays)
                                                    //DayAvg = (decimal)g.Where(m => m.x.END_DATE != null && m.x.START_DATE != null).Sum(m => (m.x.END_DATE.Value - m.x.START_DATE.Value).TotalDays) / g.Count(),
                                                    //ChapterAvg = (decimal)g.Sum(m => m.x.CHAPTERS ?? 0) / g.Count(),
                                                    //PageAvg = (decimal)g.Where(m => m.x.END_DATE != null && m.x.START_DATE != null && m.x.PAGES != null).Sum(m => m.x.PAGES) / g.Count(),
                                                };

        return model;
    }
}
