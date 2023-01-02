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
}
