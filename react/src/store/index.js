import { createStore } from "redux";

function storeReducer(state, action) {
  if (!state) {
    return {
      userLatitude: null,
      userLongitude: null,
      mapLatitude: null,
      mapLongitude: null,
      mapZoom: null
    };
  }

  if (action.type === "SET_USER_LATITUDE") {
    return {
      ...state,
      userLatitude: action.setLatitude
    };
  }

  if (action.type === "SET_USER_LONGITUDE") {
    return {
      ...state,
      userLongitude: action.setLongitude
    };
  }

  if (action.type === "SET_MAP_LATITUDE") {
    return {
      ...state,
      mapLatitude: action.setLatitude
    };
  }

  if (action.type === "SET_MAP_LONGITUDE") {
    return {
      ...state,
      mapLongitude: action.setLongitude
    };
  }

  if (action.type === "SET_MAP_ZOOM") {
    return {
      ...state,
      mapZoom: action.setZoom
    };
  }
}

export default createStore(
  storeReducer,
  window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__()
);
