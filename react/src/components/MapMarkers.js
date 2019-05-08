import React, { Component } from "react";
import { withRouter } from "react-router";
import { Popover, PopoverHeader, PopoverBody } from "reactstrap";
import "../css/custom.css";

class MapMarkers extends Component {
  state = {
    popoverOpen: false
  };

  toggle = () =>
    this.setState({
      popoverOpen: !this.state.popoverOpen
    });

  toggle = this.toggle.bind(this);

  render() {
    const { club } = this.props;
    return (
      <span>
        <button
          className="btn btn-sm btn-danger marker-btn-circle"
          color="success"
          id={"Popover-" + club.id}
          onClick={this.toggle}
        />
        <Popover
          placement={"auto"}
          isOpen={this.state.popoverOpen}
          target={"Popover-" + club.id}
          toggle={this.toggle}
          style={{ width: "17em" }}
        >
          <PopoverHeader>
            <span
              className="float-right popover-close-btn"
              onClick={this.toggle}
            >
              X
            </span>
            {club.Identification.Name}
          </PopoverHeader>
          <PopoverBody>
            <button
              className="btn btn-sm btn-danger"
              onClick={() => {
                this.props.sendClubRequest(club.id);
                this.props.setClubName(club.Identification.Name);
                this.toggle();
              }}
            >
              See Club Details
            </button>
          </PopoverBody>
        </Popover>
      </span>
    );
  }
}

export default withRouter(MapMarkers);
