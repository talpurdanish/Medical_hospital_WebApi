using AutoMapper;
using Domain.Helpers;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class ProcedureTypeManager : IProcedureTypeManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public ProcedureTypeManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ProcedureTypeViewModel?> GetProcedureType(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from p in _context.ProcedureTypes
                        where p.ProcedureTypeID == id
                        select new ProcedureTypeViewModel()
                        {
                            ID = p.ProcedureTypeID,
                            Name = p.ProcedureTypeName,
                        };

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<ProcedureTypeViewModel>> FetchProcedureTypes()
        {
            var query = from p in _context.ProcedureTypes
                        select new ProcedureTypeViewModel()
                        {
                            ID = p.ProcedureTypeID,
                            Name = p.ProcedureTypeName,
                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<ProcedureTypeViewModel>> GetProcedureTypes(DataFilter filter)
        {

            var ProcedureTypes = await FetchProcedureTypes();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, ProcedureTypes));


        }
        private static IEnumerable<ProcedureTypeViewModel> Search(string term, IEnumerable<ProcedureTypeViewModel> ProcedureType)
        {
            IEnumerable<ProcedureTypeViewModel> ProcedureTypes = new List<ProcedureTypeViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return ProcedureType;
            }
            else
            {
                var query = from u in ProcedureType
                            where
                           u.Name!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<ProcedureTypeViewModel> Sort(int field, int order, IEnumerable<ProcedureTypeViewModel> list)
        {
            IEnumerable<ProcedureTypeViewModel> listO = new List<ProcedureTypeViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),
                _ => list,
            };
            return listO;
        }


        public async Task<string> Update(ProcedureTypeViewModel viewmodel)
        {
            var medicationType = await _context.ProcedureTypes.FindAsync(viewmodel.ID);
            if (medicationType is null)
                throw new FmdcException("ProcedureType could not be found");

            medicationType.ProcedureTypeName = viewmodel.Name;
            _context.Entry(medicationType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "ProcedureType has been updated";
        }

        public async Task<string> Create(ProcedureTypeViewModel viewmodel)
        {
            var model = ProcedureTypeViewModel.GenerateModel(viewmodel);

            _context.ProcedureTypes.Add(model);
            await _context.SaveChangesAsync();
            return "ProcedureType has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var ProcedureType = await _context.ProcedureTypes.FindAsync(id);
                if (ProcedureType == null)
                {
                    return false;
                }

                _context.Entry(ProcedureType).State = EntityState.Deleted;
                _context.ProcedureTypes.Remove(ProcedureType);

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
