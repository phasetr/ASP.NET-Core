export function getAll() {
  return document.cookie;
}

export function get(key) {
  document.cookie
    .split(';')
    .forEach((cookie) => {
      const [k, v] = cookie.split('=');
      if (k === key) {
        return v;
      }
    });
  return null;
}

export function set(key, value) {
  document.cookie = `${key}=${value}`;
}
