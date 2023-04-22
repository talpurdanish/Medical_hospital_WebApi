using AutoMapper;
using Domain.Helpers;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class MedicationTypeManager : IMedicationTypeManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public MedicationTypeManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<MedicationTypeViewModel?> GetMedicationType(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from p in _context.MedicationTypes
                        where p.MedicationTypeID == id
                        select new MedicationTypeViewModel()
                        {
                            ID = p.MedicationTypeID,
                            Name = p.MedicationTypeName,
                        };

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<MedicationTypeViewModel>> FetchMedicationTypes()
        {
            var query = from p in _context.MedicationTypes
                        select new MedicationTypeViewModel()
                        {
                            ID = p.MedicationTypeID,
                            Name = p.MedicationTypeName,
                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<MedicationTypeViewModel>> GetMedicationTypes(DataFilter filter)
        {

            var MedicationTypes = await FetchMedicationTypes();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, MedicationTypes));


        }
        private static IEnumerable<MedicationTypeViewModel> Search(string term, IEnumerable<MedicationTypeViewModel> MedicationType)
        {
            IEnumerable<MedicationTypeViewModel> MedicationTypes = new List<MedicationTypeViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return MedicationType;
            }
            else
            {
                var query = from u in MedicationType
                            where
                           u.Name!.Contains(term) 
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<MedicationTypeViewModel> Sort(int field, int order, IEnumerable<MedicationTypeViewModel> list)
        {
            IEnumerable<MedicationTypeViewModel> listO = new List<MedicationTypeViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),
               

                _ => list,
            };
            return listO;
        }


        public async Task<string> Update(MedicationTypeViewModel viewmodel)
        {
            var medicationType = await _context.MedicationTypes.FindAsync(viewmodel.ID);
            if (medicationType is null)
                throw new FmdcException("MedicationType could not be found");
            medicationType.MedicationTypeName = viewmodel.Name;
            _context.Entry(medicationType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "MedicationType has been updated";
        }

        public async Task<string> Create(MedicationTypeViewModel viewmodel)
        {
            var model = MedicationTypeViewModel.GenerateModel(viewmodel);

            _context.MedicationTypes.Add(model);
            await _context.SaveChangesAsync();
            return "MedicationType has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var MedicationType = await _context.MedicationTypes.FindAsync(id);
                if (MedicationType == null)
                {
                    return false;
                }

                _context.Entry(MedicationType).State = EntityState.Deleted;
                _context.MedicationTypes.Remove(MedicationType);

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
