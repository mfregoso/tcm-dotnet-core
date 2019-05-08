import React, { Component } from "react";
import { withRouter } from "react-router";

class ResultRow extends Component {
  render() {
    const { club } = this.props;
    return (
      <React.Fragment>
        <tr>
          <td className="pointer">
            <div className="font-weight-bold">{club.Identification.Name}</div>
            <div>{club.MeetingTime}</div>
            <div>{club.MeetingDay}</div>
            <div className="font-weight-bold">
              {club.Address.Street}, {club.Address.City}
            </div>
          </td>
          <td className="align-middle">
            <button
              className="btn btn-sm btn-danger"
              onClick={() => {
                this.props.sendClubRequest(club.id);
                this.props.setClubName(club.Identification.Name);
              }}
            >
              View
            </button>
          </td>
        </tr>
      </React.Fragment>
    );
  }
}

export default withRouter(ResultRow);
