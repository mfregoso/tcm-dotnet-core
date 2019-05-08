import React, { Component } from "react";
import { Modal, ModalHeader, ModalBody } from "reactstrap";

class MetricsModal extends Component {
  titlePlaceholder = () => {
    if (this.props.memberCount || this.props.memberCount === 0) {
      return (
        <React.Fragment>
          {this.props.clubName} currently has {this.props.memberCount} members
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
          className={this.props.className}
        >
          <ModalHeader
            className="bg-danger"
            toggle={this.toggle}
            style={{ color: "white" }}
          >
            <big className="text-right float-right mx-auto ">
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
