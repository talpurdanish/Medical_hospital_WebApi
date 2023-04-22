using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class TodoManager : ITodoManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public TodoManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<TodoViewModel?> GetTodo(int id, int uId = -1)
        {
            if (id < 0)
            {
                throw new FmdcException("Id cannot be null");
            }
            var userExists = await _context.Users.AnyAsync(u => u.UserId == uId);
            if (!userExists)
                throw new FmdcException("User could not be found");
            var query = (from t in _context.TodoEvents
                         where t.UserID == uId && t.Id == id
                         select new TodoViewModel()
                         {
                             Title = t.Title,
                             Created = t.Created.ToString("dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture),
                             Completed = t.Completed
                         });
            return await query!.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TodoViewModel>> GetTodos(int id = -1)
        {
            if (id < 0)
            {
                throw new FmdcException("Id cannot be null");
            }
            var userExists = await _context.Users.AnyAsync(u => u.UserId == id);
            if (!userExists)
                throw new FmdcException("User could not be found");
            var query = (from t in _context.TodoEvents
                         where t.UserID == id
                         select new TodoViewModel()
                         {
                             id = t.Id,
                             UserId = t.UserID,
                             Title = t.Title,
                             Created = t.Created.ToString("dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture),
                             Completed = t.Completed
                         });
            return await query.ToListAsync();
        }

        public async Task<string> Create(int id, string title)
        {
            if (id < 0)
            {
                throw new FmdcException("Id cannot be null");
            }
            var userExists = await _context.Users.AnyAsync(u => u.UserId == id);
            if (!userExists)
                throw new FmdcException("User could not be found");
            if (string.IsNullOrEmpty(title))
            {
                throw new FmdcException("Title cannot be null");
            }
            var model = new TodoEvent()
            {
                Title = title,
                Created = DateTime.Now,
                Completed = false,
                UserID = id
            };

            _context.TodoEvents.Add(model);
            var addedRows = await _context.SaveChangesAsync();
            if (addedRows <= 0)
            {
                throw new FmdcException("Todo could not be created");
            }
            return "Todo has been created";

        }

        public async Task<string> Update(int id, string title)
        {

            if (string.IsNullOrEmpty(title))
            {
                throw new FmdcException("Title cannot be null");
            }
            if (id < 0)
            {
                throw new FmdcException("Id cannot be null");
            }
            var model = await _context.TodoEvents.FindAsync(id);
            if (model == null)
            {
                throw new FmdcException("Todo cannot be found");
            }
            model.Title = title;
            _context.Entry(model).State = EntityState.Modified;
            _context.TodoEvents.Update(model);
            var addedRows = await _context.SaveChangesAsync();
            if (addedRows <= 0)
            {
                throw new FmdcException("Todo could not be updated");
            }
            return "Todo has been updated";
        }

        public async Task<string> Delete(int[] ids)
        {
            foreach (var id in ids)
            {
                await Delete(id);
            }
            var addedRows = await _context.SaveChangesAsync();
            if (addedRows <= 0)
            {
                throw new FmdcException("Todos could not be deleted");
            }
            return addedRows == ids.Length ? "Some Todos have been deleted" : "Todos have been deleted";
        }

        private async Task<bool> Delete(int id)
        {
            if (id < 0)
            {
                return false;
            }
            var todo = await _context.TodoEvents.FindAsync(id);
            if (todo == null)
            {
                return false;
            }
            _context.Entry(todo).State = EntityState.Deleted;
            _context.TodoEvents.Remove(todo);

            return true;

        }

        public async Task<string> Mark(int[] ids)
        {
            foreach (var id in ids)
            {
                await Mark(id);
            }
            var addedRows = await _context.SaveChangesAsync();
            if (addedRows <= 0)
            {
                throw new FmdcException("Todo could not be marked");
            }
            return addedRows == ids.Length ? "Some Todos have been marked" : "Todos have been marked";
        }

        private async Task<bool> Mark(int id)
        {
            if (id < 0)
            {
                return false;
            }
            var todo = await _context.TodoEvents.FindAsync(id);
            if (todo == null)
            {
                return false;
            }
            todo.Completed = !todo.Completed;
            _context.Entry(todo).State = EntityState.Modified;
            _context.TodoEvents.Update(todo);

            return true;

        }




    }
}
