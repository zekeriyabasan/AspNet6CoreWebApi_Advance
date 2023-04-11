﻿using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateABook(Book entity) => Create(entity); // Methodlara RepositoryBase dekiler ile aynı isimleri vermedikki karışmasın

        public void DeleteABook(Book entity)=> Delete(entity);

        public async Task<Book> GetABookAsync(int id, bool trackChanges) => await FindByCondition(b => b.Id == id, trackChanges).SingleOrDefaultAsync();

        public async Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters, bool trackChanges) 
        {
            var books = await FindAll(trackChanges).OrderBy(b => b.Id).ToListAsync(); 

            return PagedList<Book>.ToPagedList(books,bookParameters.PageNumber, bookParameters.PageSize);
        }
           
        public void UpdateABook(Book entity) => Update(entity);
        
    }
}
