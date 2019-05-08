import axios from "axios";

export default function FindNearbyClubs(query = "", radius = 10, lat, long) {
  return axios({
    method: "get",
    url: `/api/clubs/search?query=${query}&radius=${radius}&latitude=${lat}&longitude=${long}`
  });
}
