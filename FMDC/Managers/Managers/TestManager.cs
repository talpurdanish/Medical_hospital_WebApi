using AutoMapper;
using Domain.Helpers;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class TestManager : ITestManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public TestManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<TestViewModel?> GetTest(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from t in _context.Tests
                        where t.Id == id
                        select new TestViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                        };

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<TestViewModel>> FetchTests()
        {
            var query = from t in _context.Tests
                        select new TestViewModel()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<TestViewModel>> GetTests(DataFilter filter)
        {

            var tests = await FetchTests();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, tests));


        }
        private static IEnumerable<TestViewModel> Search(string term, IEnumerable<TestViewModel> Test)
        {
            
            if (string.IsNullOrEmpty(term))
            {
                return Test;
            }
            else
            {
                var query = from u in Test
                            where
                           u.Name!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<TestViewModel> Sort(int field, int order, IEnumerable<TestViewModel> list)
        {
            IEnumerable<TestViewModel> listO = new List<TestViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),

                _ => list,
            };
            return listO;
        }


        public async Task<string> Update(TestViewModel viewmodel)
        {
            var test = await _context.Tests.FindAsync(viewmodel.Id);
            if (test is null)
                throw new FmdcException("Test could not be found");
            test.Name = viewmodel.Name;
            test.Description = viewmodel.Description;
            _context.Entry(test).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "Test has been updated";
        }

        public async Task<string> Create(TestViewModel viewmodel)
        {
            var model = TestViewModel.GenerateModel(viewmodel);

            _context.Tests.Add(model);
            await _context.SaveChangesAsync();
            return "Test has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var test = await _context.Tests.FindAsync(id);
                if (test == null)
                {
                    return false;
                }

                if (test.TestParameters.Any())
                {
                    throw new FmdcException("Please delete Test Parameters first");
                }



                _context.Entry(test).State = EntityState.Deleted;
                _context.Tests.Remove(test);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
