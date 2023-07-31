<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @editing-start="logEvent('EditingStart')"
      @init-new-row="logEvent('InitNewRow')"
      @row-inserting="logEvent('RowInserting')"
      @row-inserted="logEvent('RowInserted')"
      @row-updating="logEvent('RowUpdating')"
      @row-updated="logEvent('RowUpdated')"
      @row-removing="logEvent('RowRemoving')"
      @row-removed="logEvent('RowRemoved')"
      @saving="logEvent('Saving')"
      @saved="logEvent('Saved')"
      @edit-canceling="logEvent('EditCanceling')"
      @edit-canceled="logEvent('EditCanceled')"
  >
    <DxColumn
        data-field="login"
        data-type="string"
    />
    <DxColumn
        data-field="name"
        data-type="string"
    />
    <DxColumn
        data-field="surname"
        data-type="string"
    />
    <DxColumn
        data-field="password"
        data-type="string"
    />
    <DxColumn
        data-field="role"
        data-type="string"
    />
    <DxPaging :page-size="5"/>
    <DxPager
        :show-page-size-selector="true"
        :allowed-page-sizes="[8, 12, 20]"
    />
    <DxEditing
        :allow-deleting="true"
        :allow-adding="true"
        mode="form"
    />
  </DxDataGrid>
  <div id="events">
    <div>
      <div class="caption">
        Fired events
      </div>
      <DxButton
          id="clear"
          text="Clear"
          @click="clearEvents()"
      />
    </div>
    <ul>
      <li v-for="(event, index) in events" :key="index">{{ event }}</li>
    </ul>
  </div>
</template>

<script>
import DxButton from "devextreme-vue/button";
import {
  DxDataGrid,
  DxColumn,
  DxPager,
  DxPaging,
  DxEditing,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import userService from "@/api/userService";

const store = new CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    return await userService.getAllUsers(loadOptions);
  },
  insert: async (user) => {
    const baseResponse = await userService.createUser(user);
    return { data: baseResponse.result };
  },
  update: async (id) => {
    try {
      console.log(id + " <== тут id")
      const baseResponse = await userService.updateUser(id);
      return {data: baseResponse.result};
    } catch (error) {
      console.log(error);
    }
  },
  remove: async (oid) => {
    console.log(oid + " <== вот тут метод remove")
    const baseResponse = await userService.deleteUser(oid);
    return { data: baseResponse.result };
  },
});

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
    DxEditing,
    DxButton,
  },
  data() {
    return {
      dataSource: store,
      events: [],
    };
  },
  methods: {
    logEvent(eventName) {
      this.events.unshift(eventName);
    },
    clearEvents() {
      this.events = [];
    },
  },
};
</script>

<style>
#events {
  background-color: rgba(191, 191, 191, 0.15);
  padding: 20px;
  margin-top: 20px;
}

#events > div {
  padding-bottom: 5px;
}

#events > div::after {
  content: "";
  display: table;
  clear: both;
}

#events #clear {
  float: right;
}

#events .caption {
  float: left;
  font-weight: bold;
  font-size: 115%;
  line-height: 115%;
  padding-top: 7px;
}

#events ul {
  list-style: none;
  max-height: 100px;
  overflow: auto;
  margin: 0;
}

#events ul li {
  padding: 7px 0;
  border-bottom: 1px solid #ddd;
}

#events ul li:last-child {
  border-bottom: none;
}
</style>