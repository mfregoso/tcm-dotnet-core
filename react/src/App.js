import React, { Component } from "react";
import { Switch, Route, Redirect } from "react-router-dom";
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
          <Route render={() => <Redirect to="/" />} />
        </Switch>
      </div>
    );
  }
}

export default Routes;
