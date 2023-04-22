using AutoMapper;
using Domain.Helpers;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class MedicationManager : IMedicationManager
    {
        private readonly MedicalContext _context;
        private readonly IMapper _mapper;

        public MedicationManager(MedicalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<MedicationViewModel?> GetMedication(int id)
        {
            if (id <= 0)
                throw new FmdcException("Id is not valid");

            var query = from m in _context.Medications
                        join mt in _context.MedicationTypes on m.MedicationTypeID equals mt.MedicationTypeID
                        where m.Code == id
                        select new MedicationViewModel()
                        {
                            Code = m.Code,
                            Name = m.Name,
                            Brand = m.Brand,
                            Type = mt.MedicationTypeName,
                            TypeID = mt.MedicationTypeID,

                        };

            return await query.FirstOrDefaultAsync();
        }

        private async Task<IEnumerable<MedicationViewModel>> FetchMedications()
        {
              var query = from m in _context.Medications
                        join mt in _context.MedicationTypes on m.MedicationTypeID equals mt.MedicationTypeID
                        select new MedicationViewModel()
                        {
                            Code = m.Code,
                            Name = m.Name,
                            Brand = m.Brand,
                            Type = mt.MedicationTypeName,
                            TypeID = mt.MedicationTypeID,

                        };

            var data = await query.ToListAsync();


            return data;
        }
        public async Task<IEnumerable<MedicationViewModel>> GetMedications(DataFilter filter)
        {

            var Medications = await FetchMedications();
            return Sort(filter.SortField, filter.Order, Search(filter.Term == null ? "" : filter.Term, Medications));


        }
        private static IEnumerable<MedicationViewModel> Search(string term, IEnumerable<MedicationViewModel> Medication)
        {
            IEnumerable<MedicationViewModel> Medications = new List<MedicationViewModel>();
            if (string.IsNullOrEmpty(term))
            {
                return Medication;
            }
            else
            {
                var query = from u in Medication
                            where
                           u.Name!.Contains(term) || u.Brand!.Contains(term) || u.Type!.Contains(term)
                            select u;


                return query.ToList();
            }
        }
        private static IEnumerable<MedicationViewModel> Sort(int field, int order, IEnumerable<MedicationViewModel> list)
        {
            IEnumerable<MedicationViewModel> listO = new List<MedicationViewModel>();

            listO = field switch
            {
                1 => order == 1 ? list.OrderBy(p => p.Name) : list.OrderByDescending(p => p.Name),
                2 => order == 1 ? list.OrderBy(p => p.Brand) : list.OrderByDescending(p => p.Brand),
                3 => order == 1 ? list.OrderBy(p => p.Type) : list.OrderByDescending(p => p.Type),

                _ => list,
            };
            return listO;
        }


        public async Task<string> Update(MedicationViewModel viewmodel)
        {
            var medication = await _context.Medications.FindAsync(viewmodel.Code);
            if (medication is null)
                throw new FmdcException("Medication could not be found");
            
            medication.Description = viewmodel.Description;
            medication.Name = viewmodel.Name;
            medication.MedicationTypeID = viewmodel.TypeID;
            medication.Brand = viewmodel.Brand;
            _context.Entry(medication).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "Medication has been updated";
        }

        public async Task<string> Create(MedicationViewModel viewmodel)
        {
            var model = MedicationViewModel.GenerateModel(viewmodel);

            _context.Medications.Add(model);
            await _context.SaveChangesAsync();
            return "Medication has been created";
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var Medication = await _context.Medications.FindAsync(id);
                if (Medication == null)
                {
                    return false;
                }

                _context.Entry(Medication).State = EntityState.Deleted;
                _context.Medications.Remove(Medication);

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
