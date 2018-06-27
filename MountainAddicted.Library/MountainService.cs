using MountainAddicted.Database;
using MountainAddicted.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainAddicted.Library
{
    public class MountainService
    {
        public List<MountainViewModel> GetMountains()
        {
            using (var context = new MountainDbContext())
            {
                var mountains = context.Mountains.ToList();
                return mountains.Select(i => new MountainViewModel(i)).ToList();
            }
        }

        public MountainViewModel GetMountainData(int mountainId)
        {
            using (var context = new MountainDbContext())
            {
                var mountain = context.Mountains.FirstOrDefault(i => i.Id == mountainId);
                return GetMountainData(mountain);
            }
        }

        public MountainViewModel GetMountainData(MountainDbData mountain)
        {
            if (mountain == null)
            {
                throw new Exception("Mountain not found");
            }
            
            var previewData = DataConverter.ConvertStringToData(mountain.PreviewData);
            var originalData = DataConverter.ConvertStringToData(mountain.OriginalData);
            var viewModel = new MountainViewModel(mountain);
            viewModel.resolution = originalData?.SplitRange ?? new SplitRange { x = 100, y = 100 };
            viewModel.detailsHeights = originalData?.Points;
            viewModel.previewHeights = previewData?.Points;
            viewModel.preview = previewData?.SplitRange ?? new SplitRange { x = 5, y = 100 };
            viewModel.MountainData = originalData;
            return viewModel;
        }

        public MountainViewModel SaveMountain(MountainViewModel vm)
        {
            using(var context = new MountainDbContext())
            {
                var mountain = Save(vm, context);
                context.SaveChanges();
                return GetMountainData(mountain);
            }
        }

        public MountainViewModel SaveAndRequestHeight(MountainViewModel vm)
        {
            using (var context = new MountainDbContext())
            {
                var mountain = Save(vm, context);
                RequestHeight(mountain, context);
                return GetMountainData(mountain);
            }
        }

        private void RequestHeight(MountainDbData mountain, MountainDbContext context)
        {
            var request = new HeightRequestDbData
            {
                MountainId = mountain.Id,
                NeLat = mountain.NeLat,
                NeLng = mountain.NeLng,
                SwLat = mountain.SwLat,
                SwLng = mountain.SwLng,
                RequestNumber = 0,
                ResolutionX = 100,
                ResolutionY = 100
            };

            context.HeightRequests.Add(request);
            context.SaveChanges();
        }

        private MountainDbData Save(MountainViewModel vm, MountainDbContext context)
        {
            var mountain = context.Mountains.FirstOrDefault(i => i.Id == vm.id);
            var isNewMountain = (mountain == null);
            var isDataOutdated = false;
            if (isNewMountain)
            {
                mountain = new MountainDbData();
            }

            mountain.Title = vm.title;
            mountain.Description = vm.description;

            if (mountain.NeLat != vm.neLat || mountain.NeLng != vm.neLng ||
                mountain.SwLat != vm.swLat || mountain.SwLng != vm.swLng)
            {
                isDataOutdated = true;
            }

            mountain.NeLat = vm.neLat;
            mountain.NeLng = vm.neLng;
            mountain.SwLat = vm.swLat;
            mountain.SwLng = vm.swLng;

            mountain.PreviewData = DataConverter.ConvertDataToString(
                new MountainData
                {
                    Points = vm.previewHeights,
                    SplitRange = vm.preview
                });

            if (isDataOutdated)
            {
                mountain.OriginalData = null;
                mountain.GCodeData = null;
            }

            if (isNewMountain)
            {
                context.Mountains.Add(mountain);
            }
            context.SaveChanges();
            return mountain;
        }
    }
}
