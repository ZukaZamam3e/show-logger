using ShowLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;
public interface IBookRepository : IRepository
{
    IEnumerable<BookModel> GetBooks(Expression<Func<BookModel, bool>>? predicate = null);

    long CreateBook(int userId, BookModel model);

    long UpdateBook(int userId, BookModel model);

    bool DeleteBook(int userId, int showId);
}
