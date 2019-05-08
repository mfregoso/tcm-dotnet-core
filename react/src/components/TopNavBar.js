import React, { Component } from "react";
import { withRouter } from "react-router";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink
} from "reactstrap";

class TopNavBar extends Component {
  state = {
    isOpen: false
  };

  toggleMenu = () => this.setState({ isOpen: !this.state.isOpen });

  render() {
    const { location } = this.props;
    return (
      <React.Fragment>
        <Navbar
          color="danger"
          expand="md"
          dark={true}
          style={{ marginBottom: "3em" }}
        >
          <NavbarBrand
            href="javascript:(0)"
            onClick={() => this.props.history.push("/")}
          >
            by mfregoso
          </NavbarBrand>
          <NavbarToggler onClick={this.toggleMenu} />
          <Collapse isOpen={this.state.isOpen} navbar>
            <Nav className="ml-auto" navbar>
              <NavItem>
                <NavLink
                  href="javascript:(0)"
                  className="pointer"
                  active={true}
                  onClick={() => this.props.history.push("/find")}
                >
                  Find A Club
                </NavLink>
              </NavItem>
            </Nav>
          </Collapse>
        </Navbar>
      </React.Fragment>
    );
  }
}

export default withRouter(TopNavBar);
