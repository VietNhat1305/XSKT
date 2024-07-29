const toJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        link:item.link,
        thuTu: item.thuTu,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        link:item.link,
        thuTu: item.thuTu,
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        link:null,
        thuTu: 0,
    }
}


export const  lienKetWebsiteModel = {
    toJson, fromJson, baseJson
}
