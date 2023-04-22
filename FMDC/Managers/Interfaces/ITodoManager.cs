using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;

namespace FMDC.Managers.Interfaces
{
    public interface ITodoManager
    {
        Task<TodoViewModel?> GetTodo(int id, int uId = -1);
        Task<IEnumerable<TodoViewModel>> GetTodos(int id = -1);
        
        Task<string> Create(int id, string title);
        Task<string> Update(int id, string title);
        Task<string> Delete(int[] ids);
        Task<string> Mark(int[] ids);
        
    }
}
