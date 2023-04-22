using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace FMDC.Managers.Managers
{
    public class TestParameterManager : ITestParameterManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public TestParameterManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<TestParameterViewModel?> GetTestParameter(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from tp in _context.TestParameters
                        join t in _context.Tests on tp.TestId equals t.Id
                        where tp.Id == id
                        select new TestParameterViewModel()
                        {
                            Id = tp.Id,
                            Name = tp.Name,
                            MaleMaxValue = tp.MaleMaxValue,
                            MaleMinValue = tp.MaleMinValue,
                            FemaleMaxValue = tp.FemaleMaxValue,
                            FemaleMinValue = tp.FemaleMinValue,
                            Unit = tp.Unit,
                            TestId = tp.TestId,
                            Gender = tp.Gender,
                            ReferenceRange = tp.ReferenceRange,
                            TestName = t.Name,
                        };


            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<TestParameterViewModel>> FetchTestParameters()
        {
            var query = from tp in _context.TestParameters
                        join t in _context.Tests on tp.TestId equals t.Id
                        select new TestParameterViewModel()
                        {
                            Id = tp.Id,
                            Name = tp.Name,
                            MaleMaxValue = tp.MaleMaxValue,
                            MaleMinValue = tp.MaleMinValue,
                            FemaleMaxValue = tp.FemaleMaxValue,
                            FemaleMinValue = tp.FemaleMinValue,
                            Unit = tp.Unit,
                            TestId = tp.TestId,
                            Gender = tp.Gender,
                            ReferenceRange = tp.ReferenceRange,
                            TestName = t.Name,
                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<TestParameterViewModel>> GetTestParameters(DataFilter filter)
        {

            var TestParameters = await FetchTestParameters();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, TestParameters));


        }
        private static IEnumerable<TestParameterViewModel> Search(string term, IEnumerable<TestParameterViewModel> TestParameter)
        {
            IEnumerable<TestParameterViewModel> TestParameters = new List<TestParameterViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return TestParameter;
            }
            else
            {
                var query = from u in TestParameter
                            where
                           u.Name!.Contains(term) || u.TestName!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<TestParameterViewModel> Sort(int field, int order, IEnumerable<TestParameterViewModel> list)
        {
            IEnumerable<TestParameterViewModel> listO = new List<TestParameterViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),
                2 => order == 1 ? list.OrderBy(p => p.TestName) : list.OrderByDescending(p => p.TestName),

                _ => list,
            };
            return listO;
        }

        public async Task<IEnumerable<TestParameterViewModel>> GetTestParameters(int testId)
        {
            if (testId <= 0)
                throw new FmdcException("Id cannot be null");
            var testParameters = await FetchTestParameters();
            testParameters = testParameters.Where(d => d.TestId == testId);
            
            return testParameters;
        }

        public async Task<string> Update(TestParameterViewModel viewmodel)
        {
            var testParameter = await _context.TestParameters.FindAsync(viewmodel.Id);
            if (testParameter is null)
                throw new FmdcException("TestParameter could not be found");
            testParameter.Name = viewmodel.Name ?? "";

            testParameter.MaleMaxValue = viewmodel.MaleMaxValue;
            testParameter.MaleMinValue = viewmodel.MaleMinValue;
            testParameter.FemaleMaxValue = viewmodel.FemaleMaxValue;
            testParameter.FemaleMinValue = viewmodel.FemaleMinValue;
            testParameter.Unit = viewmodel.Unit ?? "";
            testParameter.TestId = viewmodel.TestId;
            testParameter.Gender = viewmodel.Gender;
            testParameter.ReferenceRange = viewmodel.ReferenceRange ?? "";
            _context.Entry(testParameter).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "TestParameter has been updated";
        }

        public async Task<string> Create(TestParameterViewModel viewmodel)
        {
            var model = TestParameterViewModel.GenerateModel(viewmodel);

            _context.TestParameters.Add(model);
            await _context.SaveChangesAsync();
            return "TestParameter has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var testParameter = await _context.TestParameters.FindAsync(id);
                if (testParameter == null)
                {
                    return false;
                }

                _context.Entry(testParameter).State = EntityState.Deleted;
                _context.TestParameters.Remove(testParameter);

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
