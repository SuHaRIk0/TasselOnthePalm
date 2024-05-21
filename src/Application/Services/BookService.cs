﻿using Domain.Entities;
using Domain.IRepository;
using Domain.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Domain.DTO;
using System.Net.NetworkInformation;

namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<bool> EditBookByIdAsync(int id, Book updatedBook)
        {
            var dummy = await _bookRepository.GetBookByIdAsync(id);

            if (dummy == null)
            {
                return false;
            }

            dummy.Id = updatedBook.Id;
            dummy.Title = updatedBook.Title;
            dummy.AuthorName = updatedBook.AuthorName;
            dummy.Picture = updatedBook.Picture;
            dummy.LanguageBook = updatedBook.LanguageBook;
            dummy.Genre = updatedBook.Genre;
            dummy.Description = updatedBook.Description;
            dummy.Status = updatedBook.AuthorName;

            if (dummy != null)
            {
                _logger.LogInformation("Retrivial successful!");
                return await _bookRepository.EditBookByIdAsync(dummy.Id, dummy);
            }

            _logger.LogInformation("Retrivial UNsuccessful! The result is NULL!");
            return true;
        }

        public async Task<Book?> ShowBookByIdAsync(int id)
        {
            var dummy = await _bookRepository.GetBookByIdAsync(id);

            return dummy;
        }

        public async Task<IEnumerable<Book>?> GetBooksByIdAsync(int Id)
        {
            var books = await _bookRepository.GetBooksByIdAsync(Id);
            return books;
        }

        public async Task<bool> AddAsync(Book book)
        {
            _logger.LogInformation($"started adding book operation...");

            var alreadyExistingBook = await _bookRepository.GetBookByIdAsync(book.Id);

            if (alreadyExistingBook == null)
            {
                _logger.LogInformation("Book added successfully");
                await _bookRepository.AddAsync(book);
                return true;
            }

            _logger.LogInformation($"Book already exist");
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"started delete book {id} operation...");

            var existingBook = await _bookRepository.GetBookByIdAsync(id);

            if (existingBook != null)
            {
                await _bookRepository.DeleteAsync(id);
                _logger.LogInformation($"{existingBook.Id} deleted");
                //topDbContext.Books.Remove(existingBook);
                //await topDbContext.SaveChangesAsync();
                return true;
            }

            _logger.LogInformation("Delete was unseccessful");
            return false;
        }

        //public async Task Add
    }
}