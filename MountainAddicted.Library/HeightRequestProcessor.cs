using MountainAddicted.Database;
using MountainAddicted.Library.Models;
using MountainAddicted.Library.Elevation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MountainAddicted.Library
{
    public class HeightRequestProcessor
    {
        private const int LongPause = 180000; // 180-300 sec = 3-5 min
        private const int LongDispersion = 120000; 
        private const int ShortPause = 5000; // 5-10 sec
        private const int ShortDispersion = 5000;
        private bool useLongPause;

        public void Process()
        {
            var random = new Random();
            while (true)
            {
                useLongPause = false;

                try
                {
                    using (var context = new MountainDbContext())
                    {
                        var requestToProcess = context.HeightRequests.OrderBy(i => i.Id).FirstOrDefault();
                        if (requestToProcess == null)
                        {
                            useLongPause = true;
                        }
                        else
                        {
                            var newPoints = GetData(requestToProcess);
                            var mountainData = string.IsNullOrEmpty(requestToProcess.Data) 
                                ? new MountainData
                                {
                                    Points = new List<List<MountainPoint>>(),
                                    SplitRange = new SplitRange
                                    {
                                        x = requestToProcess.ResolutionX,
                                        y = requestToProcess.ResolutionY
                                    }
                                }
                                : DataConverter.ConvertStringToData(requestToProcess.Data);
                            mountainData.Points.Add(newPoints);
                            requestToProcess.RequestNumber += 1;
                            if(requestToProcess.RequestNumber > mountainData.SplitRange.y)
                            {
                                var mountain = context.Mountains.FirstOrDefault(i => i.Id == requestToProcess.MountainId);
                                mountain.OriginalData = DataConverter.ConvertDataToString(mountainData);
                                context.HeightRequests.Remove(requestToProcess);
                            }
                            else
                            {
                                requestToProcess.Data = DataConverter.ConvertDataToString(mountainData);
                            }
                            context.SaveChanges();
                        }
                    }
                }
                catch(Exception)
                {
                    useLongPause = true;
                }

                Thread.Sleep(useLongPause 
                    ? LongPause + random.Next(LongDispersion)
                    : ShortPause + random.Next(ShortDispersion));
            }
        }

        private List<MountainPoint> GetData(HeightRequestDbData requestData)
        {
            var elevationService = new ElevationService();

            var request = new ElevationRequest();
            request.Samples = requestData.ResolutionX;

            var latStep = (requestData.NeLat - requestData.SwLat) / requestData.ResolutionY;
            var lat = requestData.SwLat + latStep * requestData.RequestNumber;
            request.Path.Add(new LatLng(lat, requestData.SwLng));
            request.Path.Add(new LatLng(lat, requestData.NeLng));

            var response = elevationService.GetResponse(request);
            if (response.Status == ServiceResponseStatus.Ok)
            {
                return response.Results.Select(i => new MountainPoint
                {
                    height = i.Elevation,
                    lat = (decimal)i.Location.Latitude,
                    lng = (decimal)i.Location.Longitude
                }).ToList();
            }
            throw new Exception("Not supported status: " + response.Status + ". " + response.ErrorMessage);
        }
    }
}
