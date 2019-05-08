import axios from "axios";

export function GetClubMetrics(id) {
  return axios({
    url: "/api/clubs/" + id,
    method: "get"
  });
}
