import axios from "axios";

const getUserLocation = () => {
  return axios({
    url:
      "https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyBcBHH4tjzT73Zms3uDoCun9GaNy-Ue5QQ",
    method: "post",
    data: {
      considerIp: true
    }
  });
};

export default getUserLocation;
