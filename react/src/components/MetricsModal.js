import React, { Component } from "react";
import { Modal, ModalHeader, ModalBody } from "reactstrap";

class MetricsModal extends Component {
  titlePlaceholder = () => {
    if (this.props.membershipCount !== null) {
      return (
        <React.Fragment>
          {this.props.clubName} currently has {this.props.membershipCount} members
        </React.Fragment>
      );
    } else {
      return "Loading club information...";
    }
  };
  render() {
    return (
      <React.Fragment>
        <Modal
          isOpen={this.props.showModal}
          size="lg"
          toggle={this.props.toggle}
        >
          <ModalHeader
            className="bg-danger"
            toggle={this.toggle}
            style={{ color: "white" }}
          >
            <big className="text-left float-left">
              {this.titlePlaceholder()}
            </big>
          </ModalHeader>
          <ModalBody style={{ paddingBottom: "1em" }}>
            {this.props.children}
          </ModalBody>
        </Modal>
      </React.Fragment>
    );
  }
}

export default MetricsModal;
