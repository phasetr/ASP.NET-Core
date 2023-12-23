export const handler = async function(event, context) {
  const credLimit = Math.floor(Math.random() * 10000);
  return (credLimit);
};
