import PropTypes from "prop-types";

export function StartExperimentButton({ link }) {
  return (
    <div className="start-experiment-container">
      <button
        className="start-experiment"
        onClick={() => (window.location.href = link)}
      >
        Start experiment
      </button>
    </div>
  );
}

StartExperimentButton.propTypes = {
  link: PropTypes.string.isRequired,
};
