export function detectEnterKey(event) {
  if (event.key === "Enter") {
    event.preventDefault();
    event.stopPropagation();
    return true;
  } else {
    return false;
  }
}
