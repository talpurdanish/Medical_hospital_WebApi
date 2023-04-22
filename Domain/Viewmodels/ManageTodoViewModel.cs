using Microsoft.CodeAnalysis;

namespace Domain.Viewmodels
{
    public class ManageTodoViewModel
    {

        public string ids{ get; set;} = "";
        public int type{ get; set;}
    }

    public class CreateTodoViewModel{ 
        public int id{ get; set;}
        public string title{ get; set;} = "";
        }
}
