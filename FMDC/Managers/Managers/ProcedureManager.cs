using AutoMapper;
using Domain.Helpers;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class ProcedureManager : IProcedureManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public ProcedureManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ProcedureViewModel?> GetProcedure(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from p in _context.Procedures
                        join pt in _context.ProcedureTypes on p.ProcedureTypeID equals pt.ProcedureTypeID
                        where p.ProcedureID == id
                        select new ProcedureViewModel()
                        {
                            ID = p.ProcedureID,
                            Name = p.ProcedureName,
                            Cost = p.ProcedureCost,
                            Type = pt.ProcedureTypeName,
                            TypeID = pt.ProcedureTypeID,

                        };

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<ProcedureViewModel>> FetchProcedures()
        {
            var query = from p in _context.Procedures
                        join pt in _context.ProcedureTypes on p.ProcedureTypeID equals pt.ProcedureTypeID
                        select new ProcedureViewModel()
                        {
                            ID = p.ProcedureID,
                            Name = p.ProcedureName,
                            Cost = p.ProcedureCost,
                            Type = pt.ProcedureTypeName,
                            TypeID = pt.ProcedureTypeID,

                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<ProcedureViewModel>> GetProcedures(DataFilter filter)
        {

            var Procedures = await FetchProcedures();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, Procedures));


        }
        private static IEnumerable<ProcedureViewModel> Search(string term, IEnumerable<ProcedureViewModel> Procedure)
        {
            IEnumerable<ProcedureViewModel> Procedures = new List<ProcedureViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return Procedure;
            }
            else
            {
                var query = from u in Procedure
                            where
                           u.Name!.Contains(term) || u.Type!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<ProcedureViewModel> Sort(int field, int order, IEnumerable<ProcedureViewModel> list)
        {
            IEnumerable<ProcedureViewModel> listO = new List<ProcedureViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),
                2 => order == 1 ? list.OrderBy(p => p.Cost) : list.OrderByDescending(p => p.Cost),
                3 => order == 1 ? list.OrderBy(p => p.Type) : list.OrderByDescending(p => p.Type),
                _ => list,
            };
            return listO;
        }


        public async Task<string> Update(ProcedureViewModel viewmodel)
        {
            var procedure = await _context.Procedures.FindAsync(viewmodel.ID);
            if (procedure is null)
                throw new FmdcException("Procedure could not be found");
            procedure.ProcedureCost = viewmodel.Cost;
            procedure.ProcedureName = viewmodel.Name;
            procedure.ProcedureTypeID = viewmodel.TypeID;
            _context.Entry(procedure).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "Procedure has been updated";
        }

        public async Task<string> Create(ProcedureViewModel viewmodel)
        {
            var model = ProcedureViewModel.GenerateModel(viewmodel);

            _context.Procedures.Add(model);
            await _context.SaveChangesAsync();
            return "Procedure has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var Procedure = await _context.Procedures.FindAsync(id);
                if (Procedure == null)
                {
                    return false;
                }

                _context.Entry(Procedure).State = EntityState.Deleted;
                _context.Procedures.Remove(Procedure);

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
