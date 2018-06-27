(function () {

    var myApp = angular.module('mountainSelection', []);

    myApp.controller('mountainSelection', ['$scope', '$http', function ($scope, $http) {
        var rectangle = null;
        var elevatorService = null;
        var elevationGettingCycle = null;
        var mapBounds = null;
        var mountains = null;
        var map = null;
        var view3d = null;
        var drawingManager = null;

        $scope.currentMountain = null;
        $scope.save = save;
        $scope.preview = preview;
        $scope.requestHeights = requestHeights;
        $scope.initMap = initMap;
        $scope.changeViewToPreview = changeViewToPreview;
        $scope.changeViewToDetails = changeViewToDetails;
        $scope.changeViewToGCode = changeViewToGCode;
        $scope.generateGCode = generateGCode;
        
        function clearCurrentMountain() {
            $scope.currentMountain = null;
        }

        function generateGCode() {
            $http(
            {
                method: "POST",
                url: 'MountainSelection/GetMountainGCode',
                data: $scope.currentMountain
            });
        }

        function setCurrentMountain(mountainInfo) {
            $scope.currentMountain = $.extend({}, defaultCurrentMountain(), mountainInfo);
            init3dview();
        }

        function loadCurrentMountainData() {
            $http(
            {
                method: "POST",
                url: 'MountainSelection/GetMountainData',
                data: $scope.currentMountain
            }).then(function (result) {
                $scope.currentMountain = result.data;
                changeViewToPreview(true);
            });
            
        }

        function changeViewToPreview(skipDataLoad) {
            var heights = $scope.currentMountain.previewHeights;
            if (!heights || !heights.length) {
                view3d.changeFigures([]);
                if (!skipDataLoad) {
                    loadCurrentMountainData();
                }
                return;
            }

            var figures = convertHeightsToFigures(heights);
            view3d.changeFigures(figures);
        }

        function changeViewToDetails() {
            var heights = $scope.currentMountain.detailsHeights;
            if (!heights || !heights.length) {
                view3d.changeFigures([]);
                return;
            }

            var figures = convertHeightsToFigures(heights);
            view3d.changeFigures(figures);
        }

        function changeViewToGCode() {
            view3d.changeFigures([]);
        }

        function convertHeightsToFigures(heights) {
            var figures = [];

            if (!heights || !heights.length) {
                return figures;
            }
            
            var maxHeight = heights.reduce(function (fig1, fig2) {
                var figMaxHeight1 = fig1.reduce(function (p1, p2) {
                    return p1.height > p2.height ? p1 : p2;
                });
                var figMaxHeight2 = fig2.reduce(function (p1, p2) {
                    return p1.height > p2.height ? p1 : p2;
                });
                return figMaxHeight1.height > figMaxHeight2.height ? [figMaxHeight1] : [figMaxHeight2];
            })[0].height;

            var minHeight = heights.reduce(function (fig1, fig2) {
                var figMaxHeight1 = fig1.reduce(function (p1, p2) {
                    return p1.height < p2.height ? p1 : p2;
                });
                var figMaxHeight2 = fig2.reduce(function (p1, p2) {
                    return p1.height < p2.height ? p1 : p2;
                });
                return figMaxHeight1.height < figMaxHeight2.height ? [figMaxHeight1] : [figMaxHeight2];
            })[0].height;

            $.each(heights, function (i, line) {
                var figure = { points: [] };
                $.each(line, function (j, point) {
                    figure.points.push({ x: i, z: j, y: (point.height - minHeight) });
                });
                figures.push(figure);
            });

            return figures;
        }

        function init3dview(skipDataLoad) {
            var view3dElement = $("#view3d");
            if (!view3d) {
                view3d = new View3d({
                    element: view3dElement
                });
            }

            changeViewToPreview(skipDataLoad);
            
            
            //figures: [
            //{ points: [{ x: -50, y: -50, z: -50}, { x: -50, y: 50, z: -50 }, { x: 50, y: 50, z: -50}, { x: 50, y: -50, z: -50}]},
            //{ points: [{ x: -50, y: -50, z: -50}, { x: 50, y: -50, z: -50 }, { x: 50, y: -50, z: 50}, { x: -50, y: -50, z: 50}]},
            //{ points: [{ x: 50, y: -50, z: -50}, { x: 50, y: 50, z: -50 }, { x: 50, y: 50, z: 50}, { x: 50, y: -50, z: 50}]},
            //{ points: [{ x: -50, y: -50, z: -50}, { x: -50, y: 50, z: -50 }, { x: -50, y: 50, z: 50 }, { x: -50, y: -50, z: 50 }] }, 
            //{ points: [{ x: -50, y: -50, z: 50}, { x: -50, y: 50, z: 50 }, { x: 50, y: 50, z: 50}, { x: 50, y: -50, z: 50}]},
            //{ points: [{ x: -50, y: 50, z: -50 }, { x: -50, y: 50, z: 50 }, { x: 50, y: 50, z: 50 }, { x: 50, y: 50, z: -50 }] }
            //]
        }

        function updateCoordinates() {
            var ne = rectangle.getBounds().getNorthEast();
            var sw = rectangle.getBounds().getSouthWest();

            $scope.currentMountain.neLat = ne.lat();
            $scope.currentMountain.neLng = ne.lng();
            $scope.currentMountain.swLat = sw.lat();
            $scope.currentMountain.swLng = sw.lng();
        }

        function defaultCurrentMountain(){
            return {
                preview: { x: 5, y: 100 },
                resolution: { x: 100, y: 100 },
                title: "(empty)",
                description: "",
                previewHeights: null
            }
        }


        function preview(){
            elevationGettingCycle = 0;
            callForHeight();
        }

        function mountainCopyWithoutData(){
            var copy = $.extend({}, $scope.currentMountain);
            copy.detailsHeights = null;
            return copy;
        }

        function save() {
            var data = { mountain: mountainCopyWithoutData() };
            $http(
            {
                method: "POST",
                url: 'MountainSelection/Save', 
                data: data
            })
            .then(function (result) {
                $scope.currentMountain = result.data;
            });
        }

        function requestHeights() {
            var data = { mountain: mountainCopyWithoutData() };
            $http(
            {
                method: "POST",
                url: 'MountainSelection/SaveAndRequestHeight',
                data: data
            })
            .then(function (result) {
                $scope.currentMountain = result.data;
            });;
        }

        function callForHeight() {

            var latSplit = $scope.currentMountain.preview.x;
            var lngSplit = $scope.currentMountain.preview.y;

            console.log(elevationGettingCycle);

            if (elevationGettingCycle > latSplit) {
                init3dview(true);
                return;
            }

            var nelat = $scope.currentMountain.neLat;
            var nelng = $scope.currentMountain.neLng;

            var swlat = $scope.currentMountain.swLat;
            var swlng = $scope.currentMountain.swLng;

            var stepLat = (nelat - swlat) / latSplit;

            var path = [];
            path.push(new google.maps.LatLng(swlat + elevationGettingCycle * stepLat, nelng));
            path.push(new google.maps.LatLng(swlat + elevationGettingCycle * stepLat, swlng));

            var pathRequest = {
                'path': path,
                'samples': lngSplit
            };
            elevatorService.getElevationAlongPath(pathRequest, elevationResults);
        }

        function elevationResults(results, status) {
            // TODO: add retry if the status is not OK.
            if (status != google.maps.ElevationStatus.OK) {
                alert("Error occured on request. Status: " + status);
                console.log("results: " + results);
                return;
            }

            if (elevationGettingCycle === 0) {
                $scope.currentMountain.previewHeights = new Array();
            }
            
            var elevations = new Array();
            for (var i = 0; i < results.length; i++) {
                elevations.push(
                {
                    height: results[i].elevation,
                    lat: results[i].location.lat(),
                    lng: results[i].location.lng()
                });
            }

            $scope.currentMountain.previewHeights[elevationGettingCycle] = elevations;

            elevationGettingCycle++;
            setTimeout(callForHeight, 200);
        }

        function initMap() {

            $http.post("MountainSelection/GetMountains", null).then(getMountainsSuccess, null);

            if (typeof (google) === "undefined") {
                setTimeout(initMap, 1000);
                return;
            }
            var mapOptions = {
                center: new google.maps.LatLng(49, 24),
                zoom: 8,
                mapTypeId: 'terrain'
            };
            map = new google.maps.Map(document.getElementById("map"), mapOptions);
            drawingManager = new google.maps.drawing.DrawingManager({
                drawingMode: google.maps.drawing.OverlayType.RECTANGLE,
                drawingControl: true,
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: ['rectangle']
                }
            });

            drawingManager.setDrawingMode(null);
            drawingManager.setMap(map);

            google.maps.event.addListener(map, "rightclick", function () {
                rectangle.setMap(null);
                clearCurrentMountain();
                $scope.$apply();
            });

            google.maps.event.addListener(drawingManager, "rectanglecomplete", function (r) {
                setupRectangle(r);
            });

            elevatorService = new google.maps.ElevationService();
        }

        function setupRectangle(r, mountainInfo) {
            if (rectangle) {
                rectangle.setMap(null);
            }
                
            rectangle = r;
                
            r.setDraggable(true);
            r.setEditable(true);
            google.maps.event.addListener(r, 'bounds_changed', function () {
                updateCoordinates();
                $scope.$apply();
            });
            drawingManager.setDrawingMode(null);
            setCurrentMountain(mountainInfo);
            updateCoordinates();
            $scope.$apply();
        }

        function getMountainsSuccess(response) {
            mountains = response.data;
            createMountainMarkers();
        }

        function createMountainMarkers() {
            if (!map) {
                setTimeout(createMountainMarkers, 200);
            }
            var markers = mountains.map(function (mountain, i) {
                var lat = (mountain.neLat + mountain.swLat) / 2;
                var lng = (mountain.neLng + mountain.swLng) / 2;
                var marker = new google.maps.Marker({
                    position: { lat: lat, lng: lng },
                    label: mountain.title
                });
                marker.addListener('click', function () {
                    var r = new google.maps.Rectangle({
                        map: map,
                        bounds: {
                            north: mountain.neLat,
                            south: mountain.swLat,
                            east: mountain.neLng,
                            west: mountain.swLng
                        }
                    });
                    setupRectangle(r, mountain);
                });
                return marker;
            });

            // Add a marker clusterer to manage the markers.
            var markerCluster = new MarkerClusterer(map, markers,
                { imagePath: 'Content/Images/m' });

        }

        initMap();
    }]);
})();