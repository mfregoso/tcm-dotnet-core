import React, { Component } from "react";
import GetUserLocation from "../services/googleGeolocate.service";
import { connect } from "react-redux";
import "../css/custom.css";

class LandingPage extends Component {
  componentDidMount() {
    if (!this.props.userLongitude && !this.props.userLatitude) {
      GetUserLocation().then(resp => {
        let { lat, lng } = resp.data.location;
        this.props.sendLatitude(lat);
        this.props.sendLongitude(lng);
      });
    } else {
      console.log(this.props.userLongitude + ", " + this.props.userLatitude);
    }
  }

  render() {
    return (
      <div
        className="col-xl-5 col-lg-6 col-md-7 col-sm-10 col-xs-12 mx-auto text-center"
        style={{ paddingBottom: "2em" }}
      >
        <h1>Toastmasters Club Metrics</h1>
        <br />
        <h4>A better way to find your new club.</h4>
        <h4>View current and past membership counts.</h4>
        <h4>See how many awards a club has achieved.</h4>
        <br />
        <button
          className="btn btn-lg btn-block btn-danger pointer"
          onClick={() => this.props.history.push("/find")}
        >
          Find Clubs Near You
        </button>
      </div>
    );
  }
}

const mapDispatchToProps = dispatch => {
  return {
    sendLatitude: setLatitude =>
      dispatch({ type: "SET_USER_LATITUDE", setLatitude }),
    sendLongitude: setLongitude =>
      dispatch({ type: "SET_USER_LONGITUDE", setLongitude })
  };
};

const mapStateToProps = state => {
  return {
    userLongitude: state.userLongitude,
    userLatitude: state.userLatitude
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(LandingPage);
