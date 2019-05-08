import React, { Component } from "react";
import { Switch, Route } from "react-router-dom";
import NavBar from "./components/TopNavBar";
import LandingPage from "./components/LandingPage";
import ViewClubs from "./components/ViewClubs";

class Routes extends Component {
  render() {
    return (
      <div>
        <NavBar />
        <Switch>
          <Route exact path="/" render={props => <LandingPage {...props} />} />
          <Route path="/find" render={props => <ViewClubs {...props} />} />
        </Switch>
      </div>
    );
  }
}

export default Routes;
