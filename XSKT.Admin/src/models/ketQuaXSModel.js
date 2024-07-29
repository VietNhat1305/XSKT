const toJson = (item) => {
  return {
    _id: item._id,
    kyVe: item.kyVe,
    date: item.date,
    giai8: item.giai8,
    giai7: item.giai7,
    giai6: item.giai6,
    giai5: item.giai5,
    giai4: item.giai4,
    giai3: item.giai3,
    giai2: item.giai2,
    giai1: item.giai1,
    giaiDB: item.giaiDB,
    file: item.file,
  };
};

const fromJson = (item) => {
  return {
    _id: item._id,
    kyVe: item.kyVe,
    date: new Date(item.date),
    giai8: item.giai8,
    giai7: item.giai7,
    giai6: item.giai6,
    giai5: item.giai5,
    giai4: item.giai4,
    giai3: item.giai3,
    giai2: item.giai2,
    giai1: item.giai1,
    giaiDB: item.giaiDB,
    file: item.file,
  };
};

const baseJson = () => {
  return {
    _id: null,
    kyVe: null,
    date: null,
    giai8: null,
    giai7: null,
    giai6: ["", "", ""],
    giai5: null,
    giai4: ["", "", "", "", "", "", ""],
    giai3: ["", ""],
    giai2: null,
    giai1: null,
    giaiDB: null,
    file: null,
  };
};

export const ketQuaXSModel = {
  toJson,
  fromJson,
  baseJson
};
