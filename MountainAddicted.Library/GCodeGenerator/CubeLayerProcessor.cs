using MountainAddicted.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MountainAddicted.Library.GCodeGenerator
{
    public class CubeLayerProcessor
    {
        public List<IGCodeMove> ProcessLayer(Layer layer, GCodeConfiguration config, decimal targetZ)
        {
            var freeze = new SpinOptions { SpinSize = config.PreparationSpinSizeMM };
            var commandList = new List<IGCodeMove>();

            var width = layer.Points.GetLength(0);
            var length = layer.Points.GetLength(1);

            do
            {
                width = width / 2;
                length = length / 2;

                var cubes = GetCubesToProcess(layer, width, length, config);
                commandList.AddRange(ProcessCubes(cubes, layer, freeze, targetZ, config));
            }
            while (width >= config.MinWidth && length >= config.MinLength);

            return commandList;
        }

        private List<IGCodeMove> ProcessCubes(List<Cube> cubes, Layer layer, SpinOptions freeze, decimal targetZ, GCodeConfiguration config)
        {
            var commands = new List<IGCodeMove>();
            foreach (var cube in cubes)
            {
                commands.AddRange(GetCommandsForCube(cube, layer, freeze, targetZ, config));
                SetLayerPointsAsProcessed(layer, cube, config);
            }
            return commands;
        }

        private List<IGCodeMove> GetCommandsForCube(Cube cube, Layer layer, SpinOptions freeze, decimal targetZ, GCodeConfiguration config)
        {
            var commands = new List<IGCodeMove>();

            var mmBetweenPointsForX = config.FormSizeMMWidth / layer.Points.GetLength(0);
            var mmBetweenPointsForY = config.FormSizeMMLength / layer.Points.GetLength(1);

            var minX = cube.X * mmBetweenPointsForX  + freeze.SpinSize / 2;
            var minY = cube.Y * mmBetweenPointsForY + freeze.SpinSize / 2;
            var maxX = (cube.X + cube.Width) * mmBetweenPointsForX - freeze.SpinSize / 2;
            var maxY = (cube.Y + cube.Length) * mmBetweenPointsForY - freeze.SpinSize / 2;

            var currentX = minX;
            var currentY = minY;
            var currentZ = targetZ - layer.SplittedMountain.MountainSplitOptions.HeightPerLayerInGCode;

            var xDirection = 1;
            var fromTopToBottom = true;

            var stepZ = config.PreparationHeightMMPerLayer;

            commands.Add(new GCodeChangeZ { Z = 0 });
            commands.Add(new GCodeMove { X = minX, Y = minY });

            do
            {
                currentZ = Math.Min(currentZ + stepZ, targetZ);
                commands.Add(new GCodeChangeZ { Z = currentZ });

                while (true)
                {
                    currentY = fromTopToBottom ? maxY : minY;
                    commands.Add(new GCodeLine { X = currentX, Y = currentY });
                    fromTopToBottom = !fromTopToBottom;

                    if ((currentX == maxX && xDirection > 0) || 
                        (currentX == minX && xDirection < 0))
                        break;

                    currentX += xDirection * freeze.SpinSize / 2;
                    if (currentX > maxX)
                        currentX = maxX;
                    if (currentX < minX)
                        currentX = minX;
                    
                    commands.Add(new GCodeLine { X = currentX, Y = currentY });
                }

                xDirection *= -1;
            }
            while (currentZ != targetZ);
            return commands;
        }

        private void SetLayerPointsAsProcessed(Layer layer, Cube cube, GCodeConfiguration config)
        {
            for (int i = 0; i < cube.Width - 2 * config.AdditionalPointsToOverflowCubesByWidth; i++)
            {
                for (int j = 0; j < cube.Length - 2 * config.AdditionalPointsToOverflowCubesByLength; j++)
                {
                    var xIndex = cube.X + i + config.AdditionalPointsToOverflowCubesByWidth;
                    var yIndex = cube.Y + j + config.AdditionalPointsToOverflowCubesByLength;
                    layer.Points[xIndex, yIndex].ProcessedStatus = ProcessedStatus.ProcessedWithBoldFreeze;
                }
            }
        }

        private List<Cube> GetCubesToProcess(Layer layer, int cubeWidth, int cubeLength, GCodeConfiguration config)
        {
            var cubes = new List<Cube>();
            var totalWidth = layer.Points.GetLength(0);
            var totalLength = layer.Points.GetLength(1);
            
            for(int i = 0; i < totalWidth / cubeWidth; i++)
            {
                for (int j = 0; j < totalLength / cubeLength; j++)
                {
                    var cube = new Cube 
                    { 
                        X = i * cubeWidth - config.AdditionalPointsToOverflowCubesByWidth, 
                        Y = j * cubeLength - config.AdditionalPointsToOverflowCubesByLength, 
                        Width = cubeWidth + 2* config.AdditionalPointsToOverflowCubesByWidth, 
                        Length = cubeLength + 2* config.AdditionalPointsToOverflowCubesByLength
                    };

                    if (NeedToProcessCube(layer, cube, totalWidth, totalLength, config))
                    {
                        cubes.Add(cube);
                    }
                }
            }
            return cubes;
        }

        private bool NeedToProcessCube(Layer layer, Cube cube, int totalWidth, int totalLength, GCodeConfiguration config)
        {
            for (int i = 0; i < cube.Width; i++)
            {
                for (int j = 0; j < cube.Length; j++)
                {
                    var xIndex = cube.X + i;
                    var yIndex = cube.Y + j;
                    if (xIndex < 0 || yIndex < 0 || xIndex >= totalWidth || yIndex >= totalLength)
                        continue;

                    var pointOfInterest = layer.Points[xIndex, yIndex];
                    if (!pointOfInterest.ShouldBeProcessed)
                    {
                        return false;
                    }
                    if (i >= config.AdditionalPointsToOverflowCubesByWidth && 
                        j >= config.AdditionalPointsToOverflowCubesByLength && 
                        i < cube.Width - 2* config.AdditionalPointsToOverflowCubesByWidth &&
                        j < cube.Length - 2* config.AdditionalPointsToOverflowCubesByLength && pointOfInterest.ProcessedStatus != ProcessedStatus.NotProcessed)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
