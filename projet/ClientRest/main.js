let map;
let vectors = [];

function createMap() {
    map = new ol.Map({
        target: 'map', // <-- This is the id of the div in which the map will be built.
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM()
            })
        ],
        view: new ol.View({
            center: ol.proj.fromLonLat([1.4437906491472496, 43.60463115151146]), // <-- Those are the GPS coordinates to center the map to.
            zoom: 10 // You can adjust the default zoom.
        })

    });
}

function getUrl(param)
{
    return "http://localhost:8737/Design_Time_Addresses/RoutingServer/Rest/" + param;
}

function findPath() {
    vectors.forEach(vector => map.removeLayer(vector));
    vectors = [];
    let startAddress = document.getElementById('startAddress').value;
    let endAddress = document.getElementById('endAddress').value;

    processCoordFromAddress([startAddress,endAddress])
}

function processCoordFromAddress(address) {
    let param = address[0].replaceAll(" ", "+");
    for (let i = 1; i < address.length; i++) {
        param += "/" + address[i].replaceAll(" ", "+")
    }
    let req = new XMLHttpRequest();
    req.open("GET", getUrl("GetCoord?address=" + param), true);
    req.setRequestHeader ("Accept", "application/json");
    req.onload = coordRecovery;
    req.send();
}

function coordRecovery() {
    if (this.status !== 200) {
        console.log("Contracts not retrieved. Check the error in the Network or Console tab.");
    }
    else {
        let responseObject = JSON.parse(this.responseText);
        let coords = responseObject.GetCoordResult.split("/")
        let params = "coord1=" + coords[0] + "&coord2=" + coords[1];
        requestToRoutingServer(params);
    }
}

function requestToRoutingServer(params) {
    let req = new XMLHttpRequest();
    req.open("GET", getUrl("FindNearestStation?"+params), true);
    req.setRequestHeader ("Accept", "application/json");
    req.onload = stationCoordRecovery;
    req.send();
}

function stationCoordRecovery() {
    if (this.status !== 200) {
        console.log("Contracts not retrieved. Check the error in the Network or Console tab.");
    }
    else {
        let responseObject = JSON.parse(this.responseText);
        createPath(responseObject.FindNearestStationResult.split('/'))
    }
}

function createPath(coords){
    let startCoords = coords[0].split('+');
    let startStationCoords = coords[1].split('+');
    let endStationCoords = coords[2].split('+');
    let endCoords = coords[3].split('+');
    constructPath(startCoords, startStationCoords, '#1e78ff')
    constructPath(startStationCoords, endStationCoords, '#ff5a1e')
    constructPath(endStationCoords, endCoords, '#1e78ff')
}

async function constructPath(startCoords, endCoords, color){
    let url = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=5b3ce3597851110001cf6248d80ebbb9aefa4fd08b3cac7ddd1b10b5&start="+startCoords[0]+","+startCoords[1]+"&end="+endCoords[0]+","+endCoords[1];
    let response = await fetch(url);
    let startCoord = await response.json();
    constructLine(startCoord.features[0].geometry.coordinates, color);
}

function constructLine(coords, color){
    // Create an array containing the GPS positions you want to draw
    let lineString = new ol.geom.LineString(coords);
    // Transform to EPSG:3857
    lineString.transform('EPSG:4326', 'EPSG:3857');
    // Create the feature
    let feature = new ol.Feature({
        geometry: lineString,
        name: 'Line'
    });
    // Configure the style of the line
    let lineStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: color,
            width: 5
        })
    });
    let source = new ol.source.Vector({
        features: [feature]
    });
    let vector = new ol.layer.Vector({
        source: source,
        style: [lineStyle]
    });
    vectors.push(vector)
    map.addLayer(vector);
}
