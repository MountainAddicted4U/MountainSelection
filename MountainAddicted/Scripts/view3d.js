
function View3d(options)
{
    var defaultOptions = {
        element: $("#view3d"),
        figures: []
    };
            
    var data = $.extend({}, defaultOptions, options);

    var container = data.element.parent()[0];

    var stats;

    var camera, controls, scene, renderer, group;
    var targetRotationOnMouseDown = 0, targetRotation = 0;
    var clock = new THREE.Clock();

    var mesh, mat;
    var windowHalfX = window.innerWidth / 2;

    var extrudeSettings = { amount: 50, bevelEnabled: true, bevelSegments: 20, steps: 20, bevelSize: 10, bevelThickness: 10 };
    var colors = [0x008000, 0x8080f0, 0xf08000, 0x008080, 0x0040f0, 0xf00000]

    init();
    animate();

    return {
        //data: data, // TODO: hide for release
        changeFigures: changeFigures
    };
    
    function init() {

        var light = new THREE.PointLight(0xffffff, 0.8);
        camera = new THREE.PerspectiveCamera(50, data.element[0].offsetWidth / data.element[0].offsetHeight, 1, 20000);
        
        controls = new THREE.FirstPersonControls(camera, data.element[0]);

        controls.movementSpeed = 1000;
        controls.lookSpeed = 0.125;
        //controls.lookVertical = true;
        //controls.constrainVertical = true;
        controls.verticalMin = 1.1;
        controls.verticalMax = 2.2;

        scene = new THREE.Scene();
        scene.background = new THREE.Color( 0xffffff );

        loadData();
        
        scene.rotateY(-Math.PI / 2);
        
        var ambientLight = new THREE.AmbientLight( 0xcccccc );
        scene.add( ambientLight );

        var directionalLight = new THREE.DirectionalLight( 0xffffff, 2 );
        directionalLight.position.set(-0.2, -0.6, 0.4).normalize();
        scene.add( directionalLight );

        renderer = new THREE.WebGLRenderer();
        renderer.setPixelRatio( window.devicePixelRatio );
        renderer.setSize(data.element[0].offsetWidth, data.element[0].offsetHeight);

        container.innerHTML = "";

        container.appendChild( renderer.domElement );

        //stats = new Stats();
        //container.appendChild( stats.dom );

        //

        container.addEventListener('mousedown', onDocumentMouseDown, false);
        container.addEventListener('touchstart', onDocumentTouchStart, false);
        container.addEventListener('touchmove', onDocumentTouchMove, false);
        window.addEventListener( 'resize', onWindowResize, false );

        camera.position.set(-11750, 5000, -53);
        //camera.add(light);
    }

    function loadData() {
        if (group) {
            scene.remove(group);
        }

        group = new THREE.Group();

        var width = data.figures.length; // 5
        $.each(data.figures, function (i, figure) {
            var length = figure.points.length; //100
            var LenWidRatio = length / width; // 20
            var pts = [];
            extrudeSettings.amount = 50 + 10 * LenWidRatio;
            pts.push(new THREE.Vector2(0, 0));
            $.each(figure.points, function (j, point) {
                pts.push(new THREE.Vector2(j * length, 10 * point.y));
            });
            pts.push(new THREE.Vector2(length * (figure.points.length - 1), 0));

            var shape = new THREE.Shape(pts);

            var geometry = new THREE.ExtrudeGeometry(shape, extrudeSettings);

            var mesh = new THREE.Mesh(geometry, new THREE.MeshPhongMaterial({ color: colors[i % colors.length] }));
            mesh.position.set(length* (-figure.points.length / 2), 0, (i - data.figures.length / 2) * width * LenWidRatio);
            mesh.scale.set(1, 1, 1);
            group.add(mesh);
        });


        var northShape = new THREE.Shape();
        northShape.moveTo(0, 3000);
        northShape.lineTo(1000, 0);
        northShape.lineTo(-1000, 0);
        northShape.lineTo(0, 3000); // close path

        var northGeometry = new THREE.ShapeBufferGeometry(northShape);
        var northMesh = new THREE.Mesh(northGeometry, new THREE.MeshPhongMaterial({ side: THREE.DoubleSide, color: "#000" }));
        northMesh.position.set(0, 7000, 0);
        northMesh.rotateX(Math.PI / 2);
        group.add(northMesh)

        scene.add(group);
        targetRotation = 2 * Math.PI;
    }

    function onDocumentMouseDown(event) {

        event.preventDefault();

        container.addEventListener('mousemove', onDocumentMouseMove, false);
        container.addEventListener('mouseup', onDocumentMouseUp, false);
        container.addEventListener('mouseout', onDocumentMouseOut, false);

        mouseXOnMouseDown = event.clientX - windowHalfX;
        targetRotationOnMouseDown = targetRotation;

    }

    function onDocumentMouseMove(event) {

        mouseX = event.clientX - windowHalfX;

        targetRotation = targetRotationOnMouseDown + (mouseX - mouseXOnMouseDown) * 0.02;

    }

    function onDocumentMouseUp(event) {

        container.removeEventListener('mousemove', onDocumentMouseMove, false);
        container.removeEventListener('mouseup', onDocumentMouseUp, false);
        container.removeEventListener('mouseout', onDocumentMouseOut, false);

    }

    function onDocumentMouseOut(event) {

        container.removeEventListener('mousemove', onDocumentMouseMove, false);
        container.removeEventListener('mouseup', onDocumentMouseUp, false);
        container.removeEventListener('mouseout', onDocumentMouseOut, false);

    }

    function onDocumentTouchStart(event) {

        if (event.touches.length == 1) {

            event.preventDefault();

            mouseXOnMouseDown = event.touches[0].pageX - windowHalfX;
            targetRotationOnMouseDown = targetRotation;

        }

    }

    function onDocumentTouchMove(event) {

        if (event.touches.length == 1) {

            event.preventDefault();

            mouseX = event.touches[0].pageX - windowHalfX;
            targetRotation = targetRotationOnMouseDown + (mouseX - mouseXOnMouseDown) * 0.05;

        }

    }


    function onWindowResize() {
        //camera.aspect = window.offsetWidth / window.offsetHeight;
        //camera.updateProjectionMatrix();

        //renderer.setSize(window.offsetWidth, window.offsetHeight);
        //windowHalfX = window.innerWidth / 2;

        controls.handleResize();
    }

    function animate() {

        requestAnimationFrame(animate);

        render();
        //stats.update();

    }

    function render() {
        controls.update(clock.getDelta());
        group.rotation.y += (targetRotation - group.rotation.y) * 0.1;
        renderer.render(scene, camera);

    }

    function changeFigures(figures) {
        data.figures = figures;
        loadData();
    }
}