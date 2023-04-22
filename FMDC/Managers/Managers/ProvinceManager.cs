using AutoMapper;
using Domain.Helpers;
using Domain.Models;
using Domain.Viewmodels;
using FMDC.Context;
using FMDC.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FMDC.Managers.Managers
{
    public class ProvinceManager : IProvinceManager
    {
        private readonly MedicalContext _context;
        public ProvinceManager(MedicalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProvinceViewModel>> GetProvinces()
        {
            var query = from province in _context.Provinces
                        select new ProvinceViewModel()
                        {
                            Name = province.Name,
                            id = province.ProvinceId
                        };
            var provinces = await query.ToListAsync();
            return provinces;
        }
    }
}
